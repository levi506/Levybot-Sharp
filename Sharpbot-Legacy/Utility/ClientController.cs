using Discord;
using Discord.Addons.Interactive;
using Discord.Webhook;
using Discord.WebSocket;
using LevyBotSharp.DataHandlers;
using LevyBotSharp.DiscordHandlers;
using LevyBotSharp.TwitchHandlers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace LevyBotSharp.Utility
{
    public static class ClientController
    {
        public static Dictionary<string, DiscordSocketClient> DiscordClients = new Dictionary<string, DiscordSocketClient>();
        public static Dictionary<ulong, string> KeysIds = new Dictionary<ulong, string>();
        public static Dictionary<ulong, InteractiveService> Interactive = new Dictionary<ulong,InteractiveService>();

        public static Dictionary<string, TwitchClient> TwitchClients = new Dictionary<string, TwitchClient>();

        const string PublicKeyDiscord = "Mocha";

        const string PublicKeyTwitch = "BerryMocha";

        public static async Task InitDiscordSocketClient(string key, string token)
        {
            DiscordClients.Add(key, new DiscordSocketClient());

            if (DiscordClients.TryGetValue(key, out var client))
            {
                //Core Events
                client.Log += async msg => { await Logger.Log(msg, key); };
                client.Ready += async () => { await Ready(client, key); };

                //Message Events
                client.ReactionAdded += async (msg, channel, reaction) => { await MainHandler.ReactionAdded(msg, channel, reaction, client); };
                client.MessageReceived += async msg => { await MainHandler.HandleMessage(msg, client); };
                client.MessageDeleted += async (msgCache, channel) => { await MainHandler.MessageDeleted(msgCache, channel, client); };

                //User Events
                client.UserJoined += async user => { await MainHandler.UserJoined(user, client); };
                //client.GuildMemberUpdated += async (old, updated) => { await MainHandler.GuildUserUpdated(old, updated, client); };
                //client.UserUpdated += async (old, updated) => { await MainHandler.UserUpdated(old, updated, client); };

                //Server Events
                client.JoinedGuild += async (guild) => { await MainHandler.JoinedGuild(guild, client); };
                client.LeftGuild += async (guild) => { await MainHandler.LeftGuild(guild, client); };
                client.GuildUpdated += async (o, n) => { await MainHandler.GuildUpdated(o, n, client); };

                //Voice Events
                //client.UserVoiceStateUpdated += async (user, old, updated) => { await MainHandler.UserVoiceStateUpdated(user, old, updated, client); };

                //Channel Events

                //Login
                await client.LoginAsync(TokenType.Bot, token);
                await client.StartAsync();


            }
            else
            {
                var cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"[ CRITICAL ] [ Discord ] {key} failed to be initialized");
                Console.ForegroundColor = cc;
            }

        }

        public static async Task InitTwitchClient(string key, string name, string oauth)
        {
            TwitchClients.Add(key, new TwitchClient());

            if (TwitchClients.TryGetValue(key, out var client))
            {
                //Core Events
                //client.OnLog += Logger.TwitchLog;  //ONLY ENABLE FOR TWITCH DEBUG PURPOSES
                client.OnConnected += async (sender, args) => await TwitchHandler.OnConneted(args, client);
                client.OnJoinedChannel += async (sender, args) => await TwitchHandler.LogJoin(args, client);

                //Message Events
                client.OnMessageReceived += async (sender, args) => await TwitchHandler.HandleMessage(args, client);
                client.OnWhisperReceived += async (sender, args) => await TwitchHandler.HandleWhisper(args, client);

                //Subscription Events
                //client.OnCommunitySubscription += async (sender, args) => await TwitchHandler.HandleComSub(args, client);
                //client.OnGiftedSubscription += async (sender, args) => await TwitchHandler.HandleGiftedSub(args, client);
                //client.OnReSubscriber += async (sender, args) => await TwitchHandler.HandleReSub(args, client);
                //client.OnNewSubscriber += async (sender, args) => await TwitchHandler.HandleNewSub(args, client);

                //Hosting Events
                //client.OnBeingHosted += async(sender, args) => await TwitchHandler.HandleHost(args, client);
                //client.OnHostingStarted += async(sender, args) => await TwitchHandler.HandleHosting(args, client);
                //client.OnRaidNotification += async(sender, args) => await TwitchHandler.HandleRaid(args, client);

                //Moderation Events
                //client.OnUserTimedout += async(sender, args) => await TwitchHandler.HandleTimeout(args, client);
                //client.OnUserBanned += async (sender, args) => await TwitchHandler.HandleBan(args, client);
                //client.OnChannelStateChanged += async (sender, args) => await TwitchHandler.HandleStateChanged(args, client);


                client.Initialize(new ConnectionCredentials(name, oauth), "berrymocha");
                client.Connect();
                await SettingsCollection.StartUp(client);
            }
            else
            {
                var cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ CRITICAL ] [ Twitch ] {key} failed to be initialized");
                Console.ForegroundColor = cc;
            }
        }

        public static Task<DiscordSocketClient> GetPublicClient()
        {
            return new Task<DiscordSocketClient>(() => DiscordClients.TryGetValue(PublicKeyDiscord, out var client) ? client : null);
        }

        public static Task<TwitchClient> GetMainClient()
        {
            return new Task<TwitchClient>(() => TwitchClients.TryGetValue(PublicKeyTwitch, out var client) ? client : null);
        }

        public static SocketUser GetOwnerDiscord()
        {
            if (DiscordClients.TryGetValue(PublicKeyDiscord, out var client))
            {
                return client.GetUser(Program.GetOwnerId());
            }
            return null;
        }

        public static DiscordSocketClient GetDiscordClient(ulong id)
        {
            KeysIds.TryGetValue(id, out var key);
            DiscordClients.TryGetValue(key, out var client);
            return client;
        }

        public static InteractiveService GetInteractiveService(this DiscordSocketClient client)
        {
            if(Interactive.TryGetValue(client.CurrentUser.Id,out var interactive))
            {
                return interactive;
            }
            else
            {
                return null;
            }
        }

        private static Task Ready(DiscordSocketClient client, string key)
        {
            if (!KeysIds.ContainsKey(client.CurrentUser.Id))
                KeysIds.Add(client.CurrentUser.Id, key);
            Interactive.Add(client.CurrentUser.Id, new InteractiveService(client));
            GuildCollection.CompileBot(client);
            return Task.CompletedTask;
        }

    }

    public struct Identifier
    {
        public ulong BotId { get; set; }
        public ulong GuildId { get; set; }
    }
}
