using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LevyBotSharp.Utility.Attributes;

namespace LevyBotSharp.TwitchHandlers.Plugins.Commands.Groups
{
    public partial class Fun : CommandGroupBase
    {
        public override string Name { get; } = "Fun";

        [CommandMeta(new[] {"Hug"}, "Hugs someone. idk what did you think this would say???")]
        public async Task HugCom(UserRequest req)
        {
            await req.SendResponseChat($"@{req.Requester.DisplayName} gives {req.Args.First().AsString()} a big hug!");
        }
    }
}
