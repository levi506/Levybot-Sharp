using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Interfaces;

namespace LevyBotSharp.DataHandlers.Models
{
    public class ChannelSettings : ITwitchSettings
    {
        public string ChannelId { get; set; }

        public string Prefix { get; set; }

        public ulong DiscordRelation { get; set; }

        public ChannelSettings(string id)
        {
            ChannelId = id;
            Prefix = "!";
        }

        public IData GetDiscordSettings()
        {
            var id = new Identifier { BotId = 455194993993449482, GuildId = DiscordRelation };
            return DiscordRelation != 0 ? GuildCollection.RequestServer(id) : null;
        }
    }
}
