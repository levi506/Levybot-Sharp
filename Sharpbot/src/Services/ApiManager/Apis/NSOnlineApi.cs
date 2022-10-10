using Sharpbot.Services.ApiManager.Utility.NSO;

namespace Sharpbot.Services.ApiManager.Apis
{
    public class NSOnlineApi
    {
        public const string USER_AGENT_VERSION = "2.0.0";
        public const string USER_AGENT_STRING = $"com.nintendo.znca/{USER_AGENT_VERSION} (Android/7.1.2)";

        public static NSOKey GKey { private get; set; }
        private Splatnet2Client Splatnet2 { get; set; }
        
        public NSOnlineApi()
        {

            Splatnet2 = new Splatnet2Client(GKey.SessionToken, GKey.CodeVerifier);
        }



    }
}
