using Discord;
using Discord.WebSocket;
using LevyBotSharp.DiscordHandlers.Plugins.Reactions;
using LevyBotSharp.DiscordHandlers.Plugins.Reactions.Starboard;
using System;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers
{
    public static partial class MainHandler
    {
        public static async Task ReactionAdded(Cacheable<IUserMessage,ulong> msgPointer, ISocketMessageChannel msgChannel, SocketReaction reaction, DiscordSocketClient client)
        {
            Console.WriteLine("Reaction");
            var msg = await msgChannel.GetMessageAsync(msgPointer.Id) as IUserMessage;
            if(msgChannel is SocketGuildChannel channel)
            {
                Console.WriteLine("Reaction Caught");
                var uReq = new ReactionRequest(reaction,msg,client);
                if (reaction.Emote.ToString() == StarboardHandler.Star.ToString())
                {
                    StarboardHandler.CheckStars(uReq);
                    //StarboardHandler.CheckGlobalStars(uReq);
                } else if (reaction.Emote.ToString() == StarboardHandler.NightSky.ToString())
                {
                    StarboardHandler.ModStar(uReq);
                }
            }
        }

        public static async Task ReactionRemoved(Cacheable<IUserMessage, ulong> msgPointer, ISocketMessageChannel msgChannel, SocketReaction reaction, DiscordSocketClient client)
        {
            Console.WriteLine("Reaction");
            var msg = await msgChannel.GetMessageAsync(msgPointer.Id) as IUserMessage;
            if (msgChannel is SocketGuildChannel channel)
            {
                Console.WriteLine("Reaction Caught");
                var uReq = new ReactionRequest(reaction, msg, client);
                if (reaction.Emote.ToString() == "⭐")
                {
                    StarboardHandler.CheckStars(uReq);
                    //StarboardHandler.CheckGlobalStars(uReq);
                }
            }
        }
    }
}
