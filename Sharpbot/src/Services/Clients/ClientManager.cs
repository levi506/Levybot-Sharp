using Discord;
using Sharpbot.Services.Clients.Utility;
using Sharpbot.Services.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpbot.Services.Clients
{
    public static class ClientManager
    {
        public static Dictionary<string, DiscordClientMask> DiscordClients { get; private set; }
        public static Dictionary<string, TwitchClientMask> TwitchClients { get; private set; }

        public static List<Client> Configs {private get; set;}


        public static void Build()
        {
            ConstructClients();
            LogManager.ProcessRawLog("Client Service", "Client Manager Initilized", LogSeverity.Info);
        }

        public static void ConstructClients()
        {
            DiscordClients = new Dictionary<string, DiscordClientMask>();
            TwitchClients = new Dictionary<string, TwitchClientMask>();

            foreach (var cli in Configs)
            {
                switch (cli.Type)
                {
                    case ClientType.Discord:
                        var disClient = new DiscordClientMask(cli);
                        DiscordClients.Add(disClient.KeyName, disClient);
                        break;
                    case ClientType.Twitch:
                        var twiClient = new TwitchClientMask(cli);
                        TwitchClients.Add(twiClient.KeyName, twiClient);
                        break;
                }

            }
        }

        internal static void Close()
        {
            throw new NotImplementedException();
        }

        public static event Func<MessageRequest,Task> OnMessageRequest;

        public static void BroadcastMsgReq(MessageRequest msg)
        {
            OnMessageRequest(msg);
        }


    }
}
