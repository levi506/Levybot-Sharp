using LevyBotSharp.Utility.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands
{
    public class Command
    {
        public List<string> Names { get; }
        public MethodInfo Function { get; }
        public CommandGroupBase ComGroup { get; }
        public bool Global { get; }
        public bool PMS { get; }
        public bool Guilds { get; }
        public string HelpUrl { get; }

        public Command(List<string> names, MethodInfo fun,CommandGroupBase group)
        {
            Names = names;
            Function = fun;
            Global = !(fun.GetCustomAttributes(typeof(GlobalContext), true).Length > 0);
            PMS = !(fun.GetCustomAttributes(typeof(PMContext), true).Length > 0);
            Guilds = !(fun.GetCustomAttributes(typeof(GuildContext), true).Length > 0);
            ComGroup = group;
        }

        public async Task Invoke(UserRequest request)
        {
            await (Task) Function.Invoke(ComGroup, new object[] {request});
        }
    }
}
