using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevyBotSharp.DataHandlers.Pipeline;
using TwitchLib.Api;
using TwitchLib.Api.Helix.Models.Games;
using TwitchLib.Api.Helix.Models.Streams;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Api.Services;

/*
Twitch Lib Docs https://swiftyspiffy.com/TwitchLib/

TwitchAPI Class https://swiftyspiffy.com/TwitchLib/Api/class_twitch_lib_1_1_api_1_1_twitch_a_p_i.html

Twitch Client Class https://swiftyspiffy.com/TwitchLib/Client/class_twitch_lib_1_1_client_1_1_twitch_client.html
*/
namespace LevyBotSharp.Utility.Apis
{
    public static class Twitch
    {

        public static TwitchAPI Api { get; private set; } = new TwitchAPI();

        public static LiveStreamMonitorService Monitor;


        public static Task Setup(string clientId, string secret)
        {
            Api.Settings.ClientId = clientId;
            Api.Settings.Secret = secret;
            Monitor = new LiveStreamMonitorService(Api);
            Monitor.OnStreamOnline += LivestreamPipe.StreamOnline;
            //Monitor.OnStreamUpdate += LivestreamPipe.StreamUpdate;
            Monitor.OnStreamOffline += LivestreamPipe.StreamOffline;
            return Task.CompletedTask;
        }

        public static async Task<User> GetUser(string username)
        {
            var user = new List<string> {username};
            var response = await Api.Helix.Users.GetUsersAsync(logins: user);

            return response.Users.Length > 0 ? response.Users.First() : null;
        }

        public static async Task<User> GetUserById(string id)
        {
            var user = new List<string> { id };
            var response = await Api.Helix.Users.GetUsersAsync(user);

            return response.Users.Length > 0 ? response.Users.First() : null;
        }

        public static async Task<Stream> GetStream(string username)
        {
            var user = new List<string> {username};
            var response = await Api.Helix.Streams.GetStreamsAsync(userLogins: user);

            return response.Streams.Length > 0 ? response.Streams.First() : null;
        }

        public static async Task<Game> GetGame(string gameid)
        {
            var game = new List<string> { gameid };
            var response = await Api.Helix.Games.GetGamesAsync(game);

            return response.Games.Length > 0 ? response.Games.First() : null;
        }

    }
}
