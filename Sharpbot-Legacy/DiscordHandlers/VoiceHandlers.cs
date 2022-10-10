using Discord.WebSocket;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers
{
    public static partial class MainHandler
    {
        public static async Task UserVoiceStateUpdated(SocketUser user, SocketVoiceState old, SocketVoiceState updated, DiscordSocketClient client)
        {
        }
    }
}
