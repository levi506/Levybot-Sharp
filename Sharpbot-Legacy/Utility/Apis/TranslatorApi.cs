using System.Threading.Tasks;
using TranslatorService;

namespace LevyBotSharp.Utility.Apis
{
    public static class TranslatorApi
    {
        private static TranslatorClient Api;

        public static void Initialize(string translatorKey)
        {
            Api = new TranslatorClient(translatorKey);
        }

        public static async Task<string> Translate(string translatee, string targetLang)
        {
            var resp = await Api.TranslateAsync(translatee, targetLang);
            
            return string.Empty;
        }
    }
}
