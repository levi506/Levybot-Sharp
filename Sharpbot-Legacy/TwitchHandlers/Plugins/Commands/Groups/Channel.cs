using System;
using System.Threading.Tasks;
using LevyBotSharp.Utility.Apis;
using LevyBotSharp.Utility.Attributes;
using TwitchLib.Api.Helix.Models.Streams;

namespace LevyBotSharp.TwitchHandlers.Plugins.Commands.Groups
{
    public class Channel : CommandGroupBase
    {
        public override string Name { get; } = "Channel";


        [CommandMeta(new [] {"Uptime"},"Displays how long a channel has been live")]
        public async Task UptimeCom(UserRequest req)
        {
            var stream = await Twitch.GetStream(req.Channel);
            if (stream != null)
            {
                var timespan = DateTime.UtcNow.Subtract(stream.StartedAt);
                if (timespan.Days > 0)
                {
                    await req.SendResponseChat(
                        $" The stream has been live for {timespan.Days} days, {timespan.Hours} hours, {timespan.Minutes} minutes, and {timespan.Seconds} seconds");
                }
                else if (timespan.Hours > 0)
                {
                    await req.SendResponseChat(
                        $" The stream has been live for {timespan.Hours} hours, {timespan.Minutes} minutes, and {timespan.Seconds} seconds");
                }
                else if (timespan.Minutes > 0)
                {
                    await req.SendResponseChat(
                        $" The stream has been live for {timespan.Minutes} minutes and {timespan.Seconds} seconds");
                }
                else
                {
                    await req.SendResponseChat(
                        $" The stream has been live for {timespan.Seconds} seconds");
                }
            } else
                await req.SendResponseChat("There is no stream up silly!");
        }

        //[CommandMeta(new[] {"Subs"}, "Gives the sub count")]
        public async Task SubsCom(UserRequest req)
        {
            
        }
    }
}
