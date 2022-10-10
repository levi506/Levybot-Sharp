using System;
using System.Collections.Generic;
using System.Linq;

namespace Sharpbot.Plugins.Commands.Utility
{
    public abstract class CommandGroup
    {
        public List<Command> Commands { get; private set; }

        public abstract string Name { get; set; }
        public List<Command> GetCommands()
        {
            var comList = new List<Command>();
            var funcs = GetType().GetNestedTypes().Where(x => x.GetCustomAttributes(typeof(CommandName), true).Length > 0 && x.GetCustomAttributes(typeof(Usage), true).Length > 0);

            foreach(var x in funcs){
                var com = Activator.CreateInstance(x) as Command;
                com.RegisterParent(this);
                comList.Add(com);
            }


            Commands = comList;
            return comList;
        }
    }
}