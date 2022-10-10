using Discord;
using Sharpbot.Plugins.Commands;
using Sharpbot.Plugins.Commands.Utility;
using Sharpbot.Services.Logger;
using Sharpbot.Services.PluginManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharpbot.Services.PluginManager
{
    public static class PluginManager
    {
        public static Dictionary<string, IPlugin> Plugins { get; private set; }


        public static async Task Build()
        {
            Plugins = new Dictionary<string, IPlugin>();

            var PluginAssemblies = typeof(PluginManager).Assembly.GetTypes().Where(x => typeof(IPlugin).IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface && x.GetCustomAttributes(typeof(PluginMeta), true).Length > 0);
            PluginAssemblies = PluginAssemblies.OrderBy(x => (x.GetCustomAttributes(typeof(PPriority) ,true).First() as PPriority).Priority);

            foreach(var plugin in PluginAssemblies)
            {
                var meta = (plugin.GetCustomAttributes(typeof(PluginMeta), true).First() as PluginMeta);
                if(meta != null)
                {
                    IPlugin plug = Activator.CreateInstance(plugin) as IPlugin;
                    Plugins.Add(meta.Name, plug);
                    LogManager.ProcessRawLog("Plugin Service", $"Loaded {meta.Name} Plugin", LogSeverity.Info);
                }

            }


        }

        internal static void Close()
        {
            //throw new NotImplementedException();
        }

        public static IPlugin GetPlugin(string name)
        {
            if(Plugins.TryGetValue(name,out var plug))
            {
                return plug;
            } 
            else
            {
                return null;
            }

        }

        public static CommandPlugin GetCommandPlugin()
        {
            if (Plugins.TryGetValue("Commands", out var plug))
            {
                return plug as CommandPlugin;
            }
            else
            {
                return null;
            }
        }
    }
}
