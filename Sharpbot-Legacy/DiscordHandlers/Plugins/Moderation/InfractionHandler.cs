using System.Threading.Tasks;
using LevyBotSharp.DataHandlers.Database;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Helpers;

namespace LevyBotSharp.DiscordHandlers.Plugins.Moderation
{
    public static class InfractionHandler
    {
        private const ulong BotId = 319015824642015264;

        public static async Task CreateWarn(ulong userId, ulong message, string reason = "", ulong modId = BotId, ulong guildId = 0)
        {
            var infract = await new InfractionBuilder
            {
                UserId = userId,
                ModeratorId = modId,
                ServerId = guildId,
                Reason = reason,
                Type = InfractType.Warn
            }.Build();

            await DataHandler.CreateInfraction(infract);
        }

        public static async Task CreateBan(ulong userId, ulong message, string reason = "", ulong modId = BotId, ulong guildId = 0)
        {
            var infract = await new InfractionBuilder
            {
                UserId = userId,
                ModeratorId = modId,
                Reason = reason,
                Type = InfractType.Ban,
                ServerId = guildId
            }.Build();

            await DataHandler.CreateInfraction(infract);
        }

        public static async Task CreateMute(ulong userId, ulong message, string reason =  "", ulong modId = BotId, ulong guildId = 0)
        {

            var infract = await new InfractionBuilder
            {
                UserId = userId,
                ModeratorId = modId,
                Reason = reason,
                Type = InfractType.Mute,
                ServerId = guildId
            }.Build();



            await DataHandler.CreateInfraction(infract);
        }

        public static async Task CreateKick(ulong userId, ulong message, string reason = "", ulong modId = BotId, ulong guildId = 0)
        {
            var infract = await new InfractionBuilder
            {
                UserId = userId,
                ModeratorId = modId,
                Reason = reason,
                Type = InfractType.Kick,
                ServerId = guildId
            }.Build();

            await DataHandler.CreateInfraction(infract);
        }
    }
}
