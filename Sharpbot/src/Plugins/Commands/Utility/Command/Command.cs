using Discord.WebSocket;
using Sharpbot.Services.Data.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharpbot.Plugins.Commands.Utility
{
    public abstract class Command
    {
        public List<string> Names { get; private set; }
        public Location Locale { get; private set; }
        protected CommandGroup Parent { get; private set; }

        public Command()
        {
            var names = GetType().GetCustomAttributes(typeof(CommandName), true);
            Names = new List<string>();

            foreach(var name in names)
            {
                var n = name as CommandName;
                Names.Add(n.Name);
            }

            var Usage = GetType().GetCustomAttributes(typeof(Usage), true).First() as Usage;
            Locale = Usage.Locale;
        }

        public bool IsProperUsage(CommandRequest req)
        {
            return (req.Loc & Locale) != 0;
        }

        public void RegisterParent(CommandGroup parentGroup)
        {
            Parent = parentGroup;
        }
        public abstract Task Excute(CommandRequest req);
        public abstract Task SendHelp(CommandRequest req);
        public abstract Task Default(CommandRequest req);

        protected SocketGuild GetDGuild(CommandRequest req)
        {
            var guildId = ulong.Parse(req.SourceRequest.ServerId);

            var guild = req.SourceRequest.DiscordBot.GetGuild(guildId);
            return guild;
        }
        protected SocketUser GetDUser(CommandRequest req)
        {
            var uId = ulong.Parse(req.SourceRequest.AuthorId);

            var user = req.SourceRequest.DiscordBot.GetUser(uId);
            return user;
        }
    }
}
