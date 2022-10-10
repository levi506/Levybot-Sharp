using System;
using System.Collections.Generic;
using System.Text;
using TwitchLib.Api.Helix.Models.Bits;

namespace LevyBotSharp.DiscordHandlers.Plugins.Pkmn.Data
{
    public class PkmnData
    {
        public ulong PkmnId { get; private set; }
        public string Nick { get; set; }
        public int Level { get; private set; }
        public ulong Exp { get; private set; }
    }
}
