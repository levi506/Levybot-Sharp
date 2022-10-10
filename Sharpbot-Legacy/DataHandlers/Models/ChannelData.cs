using System.Collections.Generic;

namespace LevyBotSharp.DataHandlers.Models
{
    public class ChannelData
    {
        public string DisplayName { get; private set; }
        public string ChannelId { get; private set; }
        private ChannelSettings Settings { get; set; }
        public List<string> ActiveBotKeys { get; private set; }
        private bool IsActive { get; set; }
    }
}
