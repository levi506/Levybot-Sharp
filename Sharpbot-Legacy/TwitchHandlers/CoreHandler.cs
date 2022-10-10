using LevyBotSharp.Utility.Apis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TwitchLib.Client;
using TwitchLib.Client.Enums;
using TwitchLib.Client.Events;
using TwitchLib.Client.Extensions;

namespace LevyBotSharp.TwitchHandlers
{
    public static partial class TwitchHandler
    {
        public static Dictionary<string, string> IdMemory { get; private set; } = new Dictionary<string,string>();

        

        public static Task OnConneted(OnConnectedArgs args, TwitchClient client)
        {
            return Task.CompletedTask;
        }

        public static Task LogJoin(OnJoinedChannelArgs args, TwitchClient client)
        {
            Console.WriteLine($"{DateTime.Now,-19} [  Twitch] [ {args.BotUsername} ] has connected to {args.Channel}");
            client.ChangeChatColor(args.Channel,ChatColorPresets.BlueViolet);
            return Task.CompletedTask;
        }

        public static async Task<string> GetId(string username)
        {
            if (IdMemory.TryGetValue(username, out var id))
            {
                return $"{id}";
            }

            var u = await Twitch.GetUser(username);
            IdMemory.Add(username, u.Id);

            return $"{u.Id}";
        }
    }
}
