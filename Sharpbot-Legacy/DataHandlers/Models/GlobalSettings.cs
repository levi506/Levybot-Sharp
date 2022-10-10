using Discord.WebSocket;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Interfaces;
using System.Threading.Tasks;

namespace LevyBotSharp.DataHandlers.Models
{
    public class GlobalSettings : ISettings
    {

        public string PriPrefix { get; private set; }

        public string SecPrefix { get; private set; }

        public bool IsLogging { get; private set; }

        public ulong StarboardChannel { get; private set; }

        public ulong ServerId { get; private set; }

        public ulong GuildId { get; }

        public ulong BotId { get; }

        public bool[] EnableLog { get; private set; }

        public ulong[] Logs { get; private set; }

        public PermLevel ValidStars { get; }

        public bool EnableStarboard { get; set; }

        public GlobalSettings(ulong botId)
        {
            PriPrefix = "!";
            SecPrefix = "?";
            IsLogging = true;
            BotId = botId;
            GuildId = 295267550986764289;
            EnableLog = new bool[16];
            for (var i = 0; i < EnableLog.Length; i++)
                EnableLog[i] = true;
            Logs = new ulong[]{
                475686079278612495,
                475678785984004096,
                475678785984004096,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619,
                475680120414404619
            };
            ValidStars = PermLevel.Helper;
            StarboardChannel = 481991100291219467;
            EnableStarboard = true;
        }

        private SocketTextChannel GetChannel(ulong channelId)
        {
            var bot = GetBot();
            var channel = bot.GetGuild(GuildId).GetTextChannel(channelId);
            return channel;
        }

        public string GetDatabaseId()
        {
            throw new System.NotImplementedException();
        }

        public SocketTextChannel GetLog(LogType log)
        {
            return GetChannel(Logs[(int)log]);
        }

        public SocketTextChannel GetStarboard()
        {
            return GetChannel(StarboardChannel);
        }

        public DiscordSocketClient GetBot()
        {
            return ClientController.GetDiscordClient(BotId);
        }

        public bool GetLogCheck(LogType type)
        {
            return true;
        }
    }

}
