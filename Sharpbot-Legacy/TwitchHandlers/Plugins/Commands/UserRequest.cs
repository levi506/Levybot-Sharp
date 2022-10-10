using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LevyBotSharp.DataHandlers.Models;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Apis;
using LevyBotSharp.Utility.Command;
using LevyBotSharp.Utility.Interfaces;
using TwitchLib.Api.Helix.Models.Users;
using TwitchLib.Client;
using TwitchLib.Client.Models;

namespace LevyBotSharp.TwitchHandlers.Plugins.Commands
{
    public enum Locale : sbyte
    {
        Global = 100,
        Whisper = 50,
        Channel = 0
    }
    public class UserRequest
    {
        public User Requester { get; private set; }
        public ChatMessage SourceMessage { get; private set; }
        public string Room { get; private set; }
        public string Channel { get; private set; }
        public TwitchClient Bot { get; private set; }
        public string Command { get; private set; }
        public List<TwitchParameter> Args { get; private set; }
        public PermLevel Permissions { get; private set; }
        public Locale Area { get; private set; }

        public UserRequest(ChatMessage msg, TwitchClient bot, ITwitchSettings settings)
        {
            var content = msg.Message;
            SourceMessage = msg;
            Room = msg.RoomId;
            Channel = msg.Channel;
            Bot = bot;

            if (content.StartsWith('g'))
            {
                Area = Locale.Global;
                content = content.Substring(1);
            }
            else
            {
                Area = Locale.Channel;
            }

            Channel = msg.Channel;

            if (content.StartsWith(settings.Prefix))
            {
                content = content.Substring(settings.Prefix.Length);
            }else {
                Command = null;
                return;
            }

            var args = content.Split(" ");
            Command = args[0];

            Args = new List<TwitchParameter>();
            for (var index = 1; index < args.Length; index++)
            {
                var arg = args[index];
                Args.Add(new TwitchParameter(arg, bot, settings.ChannelId));
            }
        }

        public static async Task<UserRequest> ReqBuilder(ChatMessage u, TwitchClient b, ITwitchSettings s)
        {
            var req = new UserRequest(u, b, s) {Requester = await Twitch.GetUser(u.Username)};
            if (req.Area > Locale.Channel)
            {
                req.Permissions = await PermissionsHandler.ResolveTGlobalPerm(req.Requester);
            }
            else
            {
                var set = s as ChannelSettings;
                req.Permissions = await PermissionsHandler.ResolveChannelPerm(u, set);
            }
            Console.WriteLine(req.Permissions);
            return req;
        }

        public Task SendResponseChat(string message)
        {
            Bot.SendMessage(Channel,message);
            return Task.CompletedTask;
        }

        public Task SendResponseWhisper(string message)
        {
            Bot.SendWhisper(Requester.Login, message);
            return Task.CompletedTask;
        }
    }

    
}
