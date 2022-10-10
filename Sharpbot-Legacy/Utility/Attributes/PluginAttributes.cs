using System;
using System.Collections.Generic;
using System.Text;

namespace LevyBotSharp.Utility.Attributes
{
    //This File is a collection of Attributes for Dynamic Loading of Plugins


    /// <summary>
    /// Attribute to set the Loading Priority of a Plugin to the Core
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PPriority : Attribute
    {
        /// <summary>
        /// Loading Priority of the Plugin
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pri">The Loading Priority of the Plugin by Core Loader<param>
        public PPriority(int priority = 1)
        {

        }
    }


    /// <summary>
    /// Attribute for the Meta Data of a Plugin for Core Referenceing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PluginMeta : Attribute
    {
        /// <summary>
        /// Plugin Namne
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Plugin Description
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Plugin Name</param>
        /// <param name="description">Plugin Description</param>
        public PluginMeta(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }
}
