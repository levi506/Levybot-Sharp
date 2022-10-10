using System;
using System.Collections.Generic;
using System.Text;

namespace LevyBotSharp.DataHandlers.Database
{
    public static class DataUtil
    {
        public static string ConstructId(ulong id, ulong idd)
        {
            var part1 = string.Format("{0:X}", id);
            if (part1.Length < 16)
            {
                part1 = "0" + part1;
            }
            var part2 = string.Format("{0:X}", idd);
            if (part2.Length < 16)
            {
                part2 = "0" + part2;
            }

            return part1 + part2;
        }
    }
}
