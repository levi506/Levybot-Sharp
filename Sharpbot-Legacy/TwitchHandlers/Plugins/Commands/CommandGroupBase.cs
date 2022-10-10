using System.Collections.Generic;
using System.Linq;
using LevyBotSharp.Utility.Attributes;

namespace LevyBotSharp.TwitchHandlers.Plugins.Commands
{
    public abstract class CommandGroupBase
    {
        public abstract string Name { get; }

        public Command[] GetCommands()
        {
            var funcs = GetType().GetMethods()
                .Where(x => x.GetCustomAttributes(typeof(CommandMetaAttribute), true).Length > 0);
            var commands = new List<Command>();

            foreach (var methodInfo in funcs)
            {
                // grab all command attributes, cast them properly
                var commandAttributes = methodInfo.GetCustomAttributes(typeof(CommandMetaAttribute), true)
                    .Select(x => x as CommandMetaAttribute).ToArray();


                // check if the function has the attribute for blocking usage in PMs
                //var forbid = (ForbidZones)methodInfo.GetCustomAttributes(typeof(ForbidZones), true).First();

                var firstCommand = commandAttributes.First();
                var command = (new Command(firstCommand.Names, true, true, true, methodInfo, this));

                commands.Add(command);
            }
            return commands.ToArray();
        }
    }
}
