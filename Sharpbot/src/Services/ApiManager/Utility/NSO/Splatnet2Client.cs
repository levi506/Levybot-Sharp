using Sharpbot.Services.ApiManager.Apis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Sharpbot.Services.ApiManager.Utility.NSO
{
    public class Splatnet2Client
    {
        private const string URI_STRING = "https://app.splatoon2.nintendo.net";
        private static Uri BASE_URI = new Uri(URI_STRING, UriKind.Absolute);
        
        private string sessionToken;
        private string codeVerifier;
        private HttpClient client { get; set; }

        public Splatnet2Client(string sessionToken, string codeVerifier)
        {
            this.sessionToken = sessionToken;
            this.codeVerifier = codeVerifier;
            client = new HttpClient();
        }

        private void getSessionCookie(string accessToken)
        {
            HttpResponseMessage repsonse;
            using (var requestMessage = new HttpRequestMessage()) {
                requestMessage.Method = HttpMethod.Get;
                requestMessage.RequestUri = BASE_URI;
                requestMessage.Headers.Add("Content-Type", "application/json; charset=utf-8");
                requestMessage.Headers.Add("X-Platform", "Android");
                requestMessage.Headers.Add("X-ProductVersion", NSOnlineApi.USER_AGENT_VERSION);
                requestMessage.Headers.Add("User-Agent", NSOnlineApi.USER_AGENT_STRING);
                requestMessage.Headers.Add("x-gamewebtoken", accessToken);
                requestMessage.Headers.Add("x-isappanalyticsoptedin", "false");
                requestMessage.Headers.Add("X-Requested-With", "com.nintendo.znca");
                requestMessage.Headers.Add("Connection", "keep-alive");
                repsonse = client.Send(requestMessage);
            }
        }

    }
}
