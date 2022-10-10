using System;
using System.Collections.Generic;
using System.Text;

namespace LevyBotSharp.Utility.Helpers
{
    public static class LinkHelper
    {
        private static Dictionary<ulong, string> DTTDic { get; set; } = new Dictionary<ulong, string>();
        private static Dictionary<string,ulong> TTDDic { get; set; } = new Dictionary<string, ulong>();

        public static void DTTProcess(ulong discordId, string targetTwitch)
        {
            if (TTDDic.ContainsValue(discordId) && TTDDic.ContainsKey(targetTwitch))
            {

            }
        }
    }
}
