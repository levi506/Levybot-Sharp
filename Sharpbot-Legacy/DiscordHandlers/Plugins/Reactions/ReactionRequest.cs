using Discord;
using Discord.WebSocket;
using LevyBotSharp.DataHandlers;
using LevyBotSharp.DataHandlers.Models;
using LevyBotSharp.DiscordHandlers.Plugins.Commands;
using LevyBotSharp.Utility;

namespace LevyBotSharp.DiscordHandlers.Plugins.Reactions
{
    public class ReactionRequest
    {
        public SocketUser Requester { get; private set; }
        public IUserMessage SourceMessage { get; private set; }
        public ISocketMessageChannel Channel { get; private set; }
        public IData Settings { get; private set; }
        public SocketGuild Guild { get; private set; }
        public DiscordSocketClient Bot { get; private set; }
        public SocketReaction Reaction { get; private set; }
        public ReactionMetadata Meta { get; private set; }
        public ulong GuildId { get; private set; }
        public Locale Area { get; private set; }

        public PermLevel perm { get; private set; }

        public ReactionRequest(SocketReaction react, IUserMessage msg, DiscordSocketClient bot)
        {
            Requester = bot.GetUser(react.UserId);
            SourceMessage = msg;
            Channel = react.Channel;
            Bot = bot;
            Reaction = react;
            var reacts = SourceMessage.Reactions;
            if (reacts != null)
            {
                reacts.TryGetValue(react.Emote, out var check);
                Meta = check;
            }

            Guild = (Channel as SocketGuildChannel)?.Guild;
            if (Guild != null)
            {
                GuildId = Guild.Id;
                var id = new Identifier { BotId = bot.CurrentUser.Id, GuildId = Guild.Id };
                Settings = GuildCollection.RequestServer(id);
                Area = Locale.Guild;
            }
            else
            {
                Settings = GuildCollection.GetGlobal(bot.CurrentUser.Id);
                Area = Locale.Global;
            }

            if(Area == Locale.Guild)
            {
                perm = ((GuildData)Settings).ResolvePerm(Guild.GetUser(Requester.Id)).GetAwaiter().GetResult();
            } else
            {
                perm = PermissionsHandler.ResolveGlobalPerm(Requester).GetAwaiter().GetResult();
            }

        }


    }
}
