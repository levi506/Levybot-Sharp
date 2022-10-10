using Discord;
using LevyBotSharp.Utility.Apis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevyBotSharp.Utility;
using TwitchLib.Api.Helix.Models.Streams;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;

namespace LevyBotSharp.DataHandlers.Pipeline
{
    public static class LivestreamPipe
    {
        private static Dictionary<string, List<ServerSub>> ServerNotif { get; set; }
        private static Dictionary<string, List<UserSub>> PersonalNotif { get; set; }
        private static Dictionary<string,bool> Recent { get; set; }

        public static void Initialize()
        {
            ServerNotif = new Dictionary<string, List<ServerSub>>();
            PersonalNotif = new Dictionary<string, List<UserSub>>();
            Recent = new Dictionary<string, bool>();

        }

        public static void StreamOnline(object sender, OnStreamOnlineArgs e)
        {
            if (Recent.TryGetValue(e.Stream.UserId,out var val) && val) return;
            Recent.Add(e.Stream.UserId,true);
            PushToServers(e.Stream);
            PushToUsers(e.Stream);
        }

        //Needs thought
        public static void StreamUpdate(object sender, OnStreamUpdateArgs e)
        {
            if (Recent.TryGetValue(e.Stream.UserId, out var val) && val) return;
            Recent.Add(e.Stream.UserId, true);
            PushToServers(e.Stream);
            PushToUsers(e.Stream);
        }
        
        public static void StreamOffline(object sender, OnStreamOfflineArgs e)
        {
            if (!Recent.TryGetValue(e.Stream.UserId, out var val)) return;
            RemoveOffline(e.Stream.UserId).Start();
        }

        private static async Task RemoveOffline(string userId)
        {
            await Task.Delay(300000);
            Recent.Remove(userId);
        }

        public static void PushToServers(Stream stream, bool update = false)
        {
            if (!ServerNotif.TryGetValue(stream.UserId, out var Notifs)) return;
            var embed = MakeStreamEmbed(stream);
            foreach (var subs in Notifs)
            {
                if (update && !subs.PushUpdates) return;
                var bot = ClientController.GetDiscordClient(subs.BotId);
                var guild = bot.GetGuild(subs.ServerId);
                if (guild == null)
                {
                    Notifs.Remove(subs);
                    continue;
                }
                var channel = guild.GetTextChannel(subs.ChannelId);
                channel?.SendMessageAsync(string.Empty, embed: embed);
            }
        }

        public static void PushToUsers(Stream stream,bool update = false)
        {
            if (!PersonalNotif.TryGetValue(stream.UserId, out var Notifs)) return;
            var embed = MakeStreamEmbed(stream);
            foreach (var subs in Notifs)
            {
                if (update && !subs.PushUpdates) return;
                var bot = ClientController.GetDiscordClient(subs.BotId);
                var user = bot.GetUser(subs.UserId);
                user?.SendMessageAsync(string.Empty,embed:embed);
            }

        }

        public static Embed MakeStreamEmbed(Stream stream, User user = null)
        {
            if (user == null)
            {
                user = Twitch.GetUserById(stream.UserId).GetAwaiter().GetResult();
            }
            var game = Twitch.GetGame(stream.GameId).GetAwaiter().GetResult();
            var embed = new EmbedBuilder()
                .WithCurrentTimestamp()
                .WithColor(100, 65, 165)
                .WithAuthor(user.DisplayName, user.ProfileImageUrl, $"https://twitch.tv/{user.DisplayName}")
                .WithDescription(user.Description)
                .AddField("Viewers", stream.ViewerCount, true)
                .WithTitle($"{stream.Title}")
                .WithUrl($"https://twitch.tv/{user.DisplayName}")//URL has a fill in the blank that must be completed
                .WithDescription(user.Description)
                .AddField("Activity", game.Name, true)
                .WithFooter($"Views: {user.ViewCount} • User Id: {user.Id}",iconUrl: "http://www.stickpng.com/assets/images/580b57fcd9996e24bc43c540.png")
                .WithImageUrl(stream.ThumbnailUrl
                   .Replace("{height}", "540")
                   .Replace("{width}", "960"))
                .WithThumbnailUrl(game.BoxArtUrl
                    .Replace("{height}", "512")
                    .Replace("{width}", "512"));

            return embed.Build();
        }
    }

    internal struct ServerSub
    {
        public ulong BotId { get; private set; }
        public ulong ServerId { get; private set; }
        public ulong ChannelId { get; private set; }
        public bool PushUpdates { get; private set; }
    }

    internal struct UserSub
    {
        public ulong BotId { get; private set; }
        public ulong UserId { get; private set; }
        public bool PushUpdates { get; private set; }
    }
}
