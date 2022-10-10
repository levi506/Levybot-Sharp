using Discord;
using Sharpbot.Services.Logger;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;

namespace Sharpbot.Services.Clients.Utility
{
    public class TwitchClientMask
    {
        public string KeyName { get; private set; }
        public string Id { get; private set; }
        private Client cli { get; set; }

        public TwitchClient Client { get; private set; }
        public DiscordClientMask Twin { get; private set; }

        public TwitchClientMask(Client Key)
        {
            KeyName = Key.Key;
            Id = Key.ID;
            cli = Key;
            Client = new TwitchClient();
            ForwardEvents();
            Client.Initialize(new ConnectionCredentials(Key.Auth.Username, Key.Auth.OAuth), Key.Auth.Username);
            Client.Connect();

            
        }

        private void ForwardEvents()
        {
            //Core Events
            //Client.OnLog += Client_OnLog; ;  //ONLY ENABLE FOR TWITCH DEBUG PURPOSES
            Client.OnConnected += Client_OnConnected;
            Client.OnJoinedChannel += Client_OnJoinedChannel;

            //Message Events
            Client.OnMessageReceived += Client_OnMessageReceived;
            Client.OnWhisperReceived += Client_OnWhisperReceived;

            //Subscription Events
            //Client.OnCommunitySubscription += async (sender, args) => await TwitchHandler.HandleComSub(args, client);
            //Client.OnGiftedSubscription += async (sender, args) => await TwitchHandler.HandleGiftedSub(args, client);
            //Client.OnReSubscriber += async (sender, args) => await TwitchHandler.HandleReSub(args, client);
            //Client.OnNewSubscriber += async (sender, args) => await TwitchHandler.HandleNewSub(args, client);

            //Hosting Events
            //Client.OnBeingHosted += async(sender, args) => await TwitchHandler.HandleHost(args, client);
            //Client.OnHostingStarted += async(sender, args) => await TwitchHandler.HandleHosting(args, client);
            //Client.OnRaidNotification += async(sender, args) => await TwitchHandler.HandleRaid(args, client);

            //Moderation Events
            //Client.OnUserTimedout += async(sender, args) => await TwitchHandler.HandleTimeout(args, client);
            //Client.OnUserBanned += async (sender, args) => await TwitchHandler.HandleBan(args, client);
            //Client.OnChannelStateChanged += async (sender, args) => await TwitchHandler.HandleStateChanged(args, client);
        }

        private void Client_OnLog(object sender, OnLogArgs e)
        {
            LogManager.ProcessRawLog("Twitch", e.Data + " " + e.DateTime, LogSeverity.Info);
        }

        private void Client_OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            ClientManager.BroadcastMsgReq(new MessageRequest(e.WhisperMessage, Client));
        }

        private void Client_OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            ClientManager.BroadcastMsgReq(new MessageRequest(e.ChatMessage, Client));
           //LogManager.ProcessRawLog("Twitch", e.ChatMessage.RawIrcMessage , LogSeverity.Verbose);
        }

        private void Client_OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            LogManager.ProcessRawLog("Twitch Client", $"{e.BotUsername} connected to {e.Channel}", LogSeverity.Info);
        }

        private void Client_OnConnected(object sender, OnConnectedArgs e)
        {
            LogManager.ProcessRawLog("Twitch Client", $"{e.BotUsername} connected to Twitch IRC", LogSeverity.Info);
            Client.JoinChannel("levi506");

        }

        public bool AttachDiscord(DiscordClientMask twin)
        {
            if (Twin == null)
            {
                Twin = twin;
                if (twin.Twin == null)
                    twin.AttachTwitch(this);
                return true;
            }
            return false;
        }


    }
}
