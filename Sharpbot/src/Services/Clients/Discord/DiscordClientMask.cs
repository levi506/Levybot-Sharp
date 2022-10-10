using Discord;
using Discord.WebSocket;
using Sharpbot.Services.Logger;
using System;
using System.Threading.Tasks;

namespace Sharpbot.Services.Clients.Utility
{
    public class DiscordClientMask
    {
        public string Id { get; private set; }
        public string KeyName { get; private set; }
        public DiscordSocketClient Client { get; private set; }
        private Client Cli;
        public TwitchClientMask Twin { get; set; }
        public DiscordClientMask(Client cli)
        {
            Cli = cli;
            KeyName = cli.Key;
            var clientConfig = new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            };
            Client = new DiscordSocketClient(clientConfig);
            ForwardEvents();
            Client.LoginAsync(TokenType.Bot, cli.Auth.OAuth);
            Client.StartAsync();
            

        }

        private void ForwardEvents()
        {
            //Core Events
            Client.Log += Log;
            Client.Ready += Ready;

            //Message Events
            Client.ReactionAdded += HandleReactionAdded;
            Client.MessageReceived += HandleMessageReceived;
            Client.MessageDeleted += HandleMessageDeleted;



            //Interaction Events
            Client.SlashCommandExecuted += HandleSlashRecieved;
            Client.ButtonExecuted += HandleButtonPressed;

            //User Events
            Client.UserJoined += HandleUserJoined;

            //Server Events
            Client.JoinedGuild += HandleJoinedGuild;
            Client.LeftGuild += HandleLeftGuild;
            Client.GuildUpdated += HandleGuildUpdated;

            //Voice Events

            //Channel Events

        }

        private Task HandleButtonPressed(SocketMessageComponent arg)
        {
            return Task.CompletedTask;
        }

        private Task HandleSlashRecieved(SocketSlashCommand arg)
        {
            
            return Task.CompletedTask;
        }

        private Task HandleGuildUpdated(SocketGuild arg1, SocketGuild arg2)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        private Task HandleLeftGuild(SocketGuild arg)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        private Task HandleJoinedGuild(SocketGuild arg)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        private Task HandleReactionAdded(Cacheable<IUserMessage, ulong> arg1, Cacheable<IMessageChannel,ulong> arg2, SocketReaction arg3)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        private Task HandleMessageReceived(SocketMessage arg)
        {
            var req = new MessageRequest(arg, Client);
            ClientManager.BroadcastMsgReq(req);
            return Task.CompletedTask;
        }

        private Task HandleMessageDeleted(Cacheable<IMessage, ulong> arg1, Cacheable<IMessageChannel,ulong> arg2)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        private Task HandleUserJoined(SocketGuildUser arg)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

        private Task Log(LogMessage arg)
        {
            LogManager.ProcessLog(arg);
            return Task.CompletedTask;
        }

        private Task Ready()
        {
            LogManager.ProcessRawLog("Discord Client", $"{KeyName} is Ready", LogSeverity.Info);
            ApiManager.ApiManager.GetLavalinkClient(Client);
            return Task.CompletedTask;
        }

        public bool AttachTwitch(TwitchClientMask twin)
        {
            if(twin != null)
            {
                Twin = twin;
                if(twin.Twin == null)
                    twin.AttachDiscord(this);
                return true;
            }
            return false;
        }
    }
}
