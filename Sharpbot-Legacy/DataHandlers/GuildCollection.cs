using Discord.WebSocket;
using LevyBotSharp.DataHandlers.Models;
using LevyBotSharp.Utility;
using System.Collections.Generic;

namespace LevyBotSharp.DataHandlers
{
    public static class GuildCollection
    {
        private static Dictionary<Identifier, GuildData> DataPile { get; set; }
        private static Dictionary<ulong, GlobalData> BotGlobals { get; set; }

        public static void CompileBot(DiscordSocketClient bot)
        {
            if (DataPile == null)
                DataPile = new Dictionary<Identifier, GuildData>();
            if (BotGlobals == null)
                BotGlobals = new Dictionary<ulong, GlobalData>();
            var botId = bot.CurrentUser.Id;
            foreach (var guild in bot.Guilds)
            {
                var id = new Identifier
                {
                    BotId = botId,
                    GuildId = guild.Id
                };
                DataPile.TryAdd(id, new GuildData(bot, guild));
            }
            BotGlobals.TryAdd(bot.CurrentUser.Id, new GlobalData(bot));
        }

        public static GuildData RequestServer(Identifier id)
        {
            return !DataPile.TryGetValue(id, out var data) ? InitServer(id) : data;

        }

        public static GuildData InitServer(Identifier id)
        {
            var bot = ClientController.GetDiscordClient(id.BotId);
            var guild = bot.GetGuild(id.GuildId);
            var data = new GuildData(bot, guild);
            DataPile.Add(id, data);
            return data;
        }

        public static GlobalData GetGlobal(ulong botId)
        {
            BotGlobals.TryGetValue(botId, out var data);
            return data;
        }

    }
}
