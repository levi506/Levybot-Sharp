using Discord.WebSocket;
using Sharpbot.Services.ApiManager.Apis;
using Sharpbot.Services.Logger;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sharpbot.Services.ApiManager
{
    public static class ApiManager
    {
        public static YoutubeApi GYoutube { get; private set; }
        public static TwitchApi GTwitch { get; private set; }
        public static NSOnlineApi GNSOnline { get; private set; }

        public static Dictionary<string,Lavalink> LavalinkClients { get; private set; }
        public static Dictionary<string,TwitchApi> TwitchClients { get; private set; }
        public static Dictionary<string,YoutubeApi> YoutubeClients { get; private set; }

        internal static async Task Build()
        {
            GTwitch = new TwitchApi();
            GYoutube = new YoutubeApi();
            GNSOnline = new NSOnlineApi();
            LavalinkClients = new Dictionary<string, Lavalink>();
            TwitchClients = new Dictionary<string, TwitchApi>();
            YoutubeClients = new Dictionary<string, YoutubeApi>();
            LogManager.ProcessRawLog("Api Service", "Api Manager Intialized", Discord.LogSeverity.Info);
        }

        public static async Task<Lavalink> GetLavalinkClient(BaseSocketClient bot)
        {
            if (LavalinkClients.TryGetValue(bot.CurrentUser.Id.ToString(), out var lavalink))
            {
                return lavalink;
            } 
            return MakeLavalinkClient(bot);
        }

        public static Lavalink MakeLavalinkClient(BaseSocketClient DiscordBot)
        {
            var lavalink = new Lavalink(DiscordBot);
            if (LavalinkClients.ContainsKey(DiscordBot.CurrentUser.Id.ToString()))
            {
               LavalinkClients.Remove(DiscordBot.CurrentUser.Id.ToString());
            }
            LavalinkClients.Add(DiscordBot.CurrentUser.Id.ToString(), lavalink);
            return lavalink;
        }

        internal static void Close()
        {
            throw new NotImplementedException();
        }
    }
}
