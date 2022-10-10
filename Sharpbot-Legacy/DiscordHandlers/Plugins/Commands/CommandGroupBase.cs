using System.Collections.Generic;
using System.Linq;
using LevyBotSharp.Utility.Attributes;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands
{
    public abstract class CommandGroupBase
    {
        public abstract string Name { get; }
        //public abstract string Classification { get; }

        //public abstract Plugin Parent { get; }
        

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


                var firstCommand = commandAttributes.First();
                Command command = new Command(firstCommand.Names, methodInfo, this);

                commands.Add(command);
            }
            return commands.ToArray();
        }

        /*public Command[] ConstructCommands()
        {
            var Commands = GetType().GetNestedTypes().Where(x => x.GetCustomAttributes(typeof(CommandAttribute), true).Length > 0);
            return new Command[0];
        }*/
    }

}
