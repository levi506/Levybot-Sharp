using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using LevyBotSharp.DataHandlers.Database;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Interfaces;

namespace LevyBotSharp.DataHandlers.Models
{

    public class GuildSettings : ISettings
    {

        const int LogCount = 16;  

        public string PriPrefix { get;  set; }

        public string SecPrefix { get; set; }

        public DateTime Created { get; set; }

        public ulong GuildId { get; set; }

        public ulong BotId { get; private set; }

        public bool IsLogging { get; set; }

        public bool[] EnableLog { get; set; }

        public ulong[] Logs { get; set; }


        //Starboard
        public PermLevel ValidStars { get; set; }

        public bool EnableStarboard { get; set; }

        public ulong StarboardChannel { get; set; }

        public GuildSettings(ulong Id)
        {
            PriPrefix = "!";
            SecPrefix = null;
            GuildId = Id;
            EnableStarboard = false;
            Created = DateTime.UtcNow;
            initLogs();
        }

        public GuildSettings() { }

        public void initLogs()
        {
            Logs = new ulong[LogCount];
            EnableLog = new bool[LogCount];
        }
        private SocketTextChannel GetChannel(ulong channelId, DiscordSocketClient bot)
        {
            var channel = bot.GetGuild(GuildId).GetTextChannel(channelId);
            return channel;
        }

        public string GetDatabaseId()
        {
            return DataUtil.ConstructId(GuildId, BotId);
        }

        public SocketTextChannel GetLog(LogType log)
        {
            var bot = GetBot();
            return GetChannel(Logs[(int)log], bot);
        }

        public bool GetLogCheck(LogType log)
        {
            return EnableLog[(int)log];
        }

        public SocketTextChannel GetStarboard()
        {
            var bot = GetBot();
            return GetChannel(StarboardChannel, bot);
        }

        public DiscordSocketClient GetBot()
        {
            return ClientController.GetDiscordClient(BotId); 
        }

        internal void SetBotId(ulong v)
        {
            if(BotId == 0)
            {
                BotId = v;
            }
        }
    }

}
