using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LevyBotSharp.Localization
{
    public static class StrHandler
    {
        public static Dictionary<string, Dictionary<string, string>> strings;

        public static async Task InitHandler()
        {
            strings = new Dictionary<string, Dictionary<string,string>>();
            var codes = await LangHandler.GetCodes();
            var langs = await LangHandler.GetLangauges();

            foreach(var lang in langs)
            {
            }

        }

        public static async Task<string> GetString(string key)
        {
            return string.Empty;
        }
    }
}
