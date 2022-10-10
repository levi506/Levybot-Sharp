using TwitchLib.Api;
using TwitchLib.Api.Services;


/*
Twitch Lib Docs https://swiftyspiffy.com/TwitchLib/

TwitchAPI Class https://swiftyspiffy.com/TwitchLib/Api/class_twitch_lib_1_1_api_1_1_twitch_a_p_i.html

Twitch Client Class https://swiftyspiffy.com/TwitchLib/Client/class_twitch_lib_1_1_client_1_1_twitch_client.html
*/

namespace Sharpbot.Services.ApiManager.Apis
{
    public class TwitchApi
    {
        public static TwitchKey GKey{private get; set;}
        public TwitchAPI Api { private get; set; }
        public LiveStreamMonitorService Monitor { get; private set; }

        public TwitchApi()
        {
            Api = new TwitchAPI();
            Api.Settings.ClientId = GKey.ClientId;
            Api.Settings.Secret = GKey.Secret;
            Monitor = new LiveStreamMonitorService(Api);
            
        }
        public TwitchApi(TwitchKey key)
        {
            Api = new TwitchAPI();
            Api.Settings.ClientId = key.ClientId;
            Api.Settings.Secret = key.Secret;
            Monitor = new LiveStreamMonitorService(Api);

        }
    }
}
