using Discord.WebSocket;
using LevyBotSharp.Utility.Helpers;
using System;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers
{
    public static partial class MainHandler
    {
        
        public static async Task GuildUserUpdated(SocketGuildUser o, SocketGuildUser n, DiscordSocketClient b)
        {
            Console.WriteLine($"Guild User Updated Event Fired! for {n.DisplayName()}");
        }

        public static async Task UserUpdated(SocketUser o, SocketUser n, DiscordSocketClient b)
        {
            Console.WriteLine($"User Updated Event Fired! for {n.Username}");
        }

        public static async Task UserJoined(SocketGuildUser o, DiscordSocketClient b)
        {
        }
    }
}
