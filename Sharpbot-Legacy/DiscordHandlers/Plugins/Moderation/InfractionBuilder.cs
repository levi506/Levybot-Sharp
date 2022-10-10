using Discord;
using System;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Moderation
{
    class InfractionBuilder
    {
        public ulong ModeratorId { get; set; }

        public ulong UserId { get; set; }

        public ulong ServerId { get; set; }

        public ulong MessageId { get; set; }

        public string Reason { get; set; }

        internal const ulong BotServerId = 295267550986764289;

        internal const ulong BotId = 319015824642015264;

        public InfractType Type;

        public async Task<Infraction> Build()
        {
            var infract = new Infraction();
            if (ServerId != 0)
                infract.ServerId = ServerId;
            else
                infract.ServerId = BotServerId;

            if (ModeratorId != 0)
                infract.ModId = ModeratorId;
            else
                infract.ModId = BotId;

            infract.MessageId = MessageId;

            infract.UserId = UserId;

            infract.Reason = Reason;

            infract.Type = Type;

            return infract;
        }



    }
}
