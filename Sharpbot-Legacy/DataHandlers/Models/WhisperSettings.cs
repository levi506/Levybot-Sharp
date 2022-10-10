using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Interfaces;

namespace LevyBotSharp.DataHandlers.Models
{
    public class WhisperSettings : ITwitchSettings
    {
        public string ChannelId { get; set; }
        public string Prefix { get; set; }
        public ulong DiscordRelation { get; set; }

        public WhisperSettings()
        {
            ChannelId = "0";
            Prefix = "!";
            DiscordRelation = 0UL;
        }

        public IData GetDiscordSettings()
        {
            return GuildCollection.GetGlobal(ClientController.GetPublicClient().GetAwaiter().GetResult().CurrentUser.Id);
        }
    }
}
