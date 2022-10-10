using System;
using System.Collections.Generic;

namespace LevyBotSharp.Utility.Attributes
{

    [AttributeUsage(AttributeTargets.Method)]
    public class CommandMetaAttribute : Attribute
    {
        public List<string> Names { get; }

        public string ShortHelp { get; }

        public string Group { get; }

        public CommandMetaAttribute(string[] names, string shortHelp = "Yet to be written", string group = "Unsorted")
        {
            Names = new List<string>();
            foreach (var name in names)
            {
                Names.Add(name);
            }
            ShortHelp = shortHelp;
            Group = group;
        }
    }


    [AttributeUsage(AttributeTargets.Method)]
    public class GlobalContext : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PMContext : Attribute
    {

    }

    [AttributeUsage(AttributeTargets.Method)]
    public class GuildContext : Attribute
    {

    }
    public class State
    {
        [AttributeUsage(AttributeTargets.Method)]
        public class Released : Attribute
        {

        }

        [AttributeUsage(AttributeTargets.Method)]
        public class Indev : Attribute
        {

        }

        [AttributeUsage(AttributeTargets.Method)]
        public class Bugged : Attribute
        {

        }

        [AttributeUsage(AttributeTargets.Method)]
        public class Testing : Attribute
        {

        }
    }
}
