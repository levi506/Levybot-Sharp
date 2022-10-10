using System;
using System.Collections.Generic;
using System.Text;

namespace Sharpbot.Services.PluginManager.Utility
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginMeta : Attribute
    {
        public string Name;
        public string Description;

        public PluginMeta(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class PPriority : Attribute
    {
        public int Priority { get; }
        
        public PPriority(int priority = 5)
        {
            Priority = priority;
        }
    }
}
