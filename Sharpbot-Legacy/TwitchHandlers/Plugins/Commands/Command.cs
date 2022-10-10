using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace LevyBotSharp.TwitchHandlers.Plugins.Commands
{
    public class Command
    {
        public List<string> Names { get; }
        public MethodInfo Function { get; }
        public CommandGroupBase ComGroup { get; }
        public bool Global { get; }
        public bool Whisper { get; }
        public bool Channel { get; }
        public string HelpUrl { get; }

        public Command(List<string> names, bool glob, bool whisp, bool channel, MethodInfo fun, CommandGroupBase group)
        {
            Names = names;
            Function = fun;
            Global = glob;
            Whisper = whisp;
            Channel = channel;
            ComGroup = group;
            HelpUrl = "https://levi506.net/bot/twitch#" + names.First();
        }

        public async Task Invoke(UserRequest request)
        {
            await (Task)Function.Invoke(ComGroup, new object[] { request });
        }
    }
}
