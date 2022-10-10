using Discord.WebSocket;

namespace LevyBotSharp.Utility.Helpers
{
    public static class GuildUserExtensions
    {
        public static string DisplayName(this SocketGuildUser u)
        {
            return u.Nickname ?? u.Username;
        }
    }
}
