using System;
using System.Threading.Tasks;
using LevyBotSharp.Utility.Attributes;

namespace LevyBotSharp.TwitchHandlers.Plugins.Commands.Groups
{
    public class Debug : CommandGroupBase
    {
        public override string Name { get; } = "Debug";

        [CommandMeta(new[] { "Ping" }, "Pong!")]
        public async Task UptimeCom(UserRequest req)
        {
            await req.SendResponseChat("Pong!");
        }

        [CommandMeta(new[] {"Link"}, "Link a Discord Account")]
        public async Task LinkDiscordCom(UserRequest req)
        {

        }
    }
}
