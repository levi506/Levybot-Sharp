using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LevyBotSharp.Localization
{
    public static class LangHandler
    {
        public async static Task<List<Dictionary<string,string>>> GetLangauges()
        {

            var dir = Directory.GetCurrentDirectory() + "/Languages/";
            var files = Directory.GetFiles(dir);

            foreach(var file in files)
            {
                if (!file.EndsWith(".l10n")) continue;



            }


            return new List<Dictionary<string, string>>();
        }

        public async static Task<List<string>> GetCodes()
        {


            return new List<string>();
        }
    }
}

