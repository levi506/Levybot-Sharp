using Sharpbot.Services.Data.Utility;
using System;

namespace Sharpbot.Plugins.Commands.Utility
{

    [AttributeUsage(AttributeTargets.Class,AllowMultiple = true)]
    public class CommandName : Attribute{
        public string Name { get; private set; }

        public CommandName(string name)
        {
            Name = name;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandMeta : Attribute
    {

        public CommandMeta(string description,string group = "Undefined")
        {
            Description = description;
            Group = group;
        }
        public string Description { get; private set; }
        public string Group { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class Usage: Attribute
    {
        public Location Locale { get; }

        public Usage(Location loc)
        {
            Locale = loc;
        }
    }
}
