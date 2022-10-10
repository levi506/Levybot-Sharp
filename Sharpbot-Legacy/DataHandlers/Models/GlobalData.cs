using Discord.WebSocket;
using LevyBotSharp.Utility.Interfaces;

namespace LevyBotSharp.DataHandlers.Models
{
    public class GlobalData : IData
    {
        public DiscordSocketClient Bot { get; }

        public ulong GuildId { get; set; } = 295267550986764289;

        public ISettings SettCache { get; }

        public GlobalData(DiscordSocketClient bot)
        {
            Bot = bot;
            GuildId = 295267550986764289;
            SettCache = new GlobalSettings(bot.CurrentUser.Id);
        }

        public SocketGuildChannel GetLog(LogType type)
        {
            return SettCache.GetLog(type);
        }
    }
}
