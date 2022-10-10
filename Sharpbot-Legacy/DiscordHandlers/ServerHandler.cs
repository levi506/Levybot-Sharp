using Discord.WebSocket;
using LevyBotSharp.DataHandlers;
using LevyBotSharp.Utility;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers
{
    public static partial class MainHandler
    {

        public static async Task JoinedGuild(SocketGuild guild, DiscordSocketClient client)
        {
            GuildCollection.RequestServer(new Identifier { BotId = client.CurrentUser.Id, GuildId = guild.Id });
        }

        public static async Task LeftGuild(SocketGuild guild, DiscordSocketClient client)
        {
        }

        public static async Task GuildUpdated(SocketGuild o, SocketGuild n, DiscordSocketClient client)
        {
        }
    }
}
