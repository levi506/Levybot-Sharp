using Sharpbot.Plugins.Commands.Commands;
using Sharpbot.Plugins.Commands.Utility;
using Sharpbot.Services.Clients;
using Sharpbot.Services.Clients.Utility;
using Sharpbot.Services.Data.Utility;
using Sharpbot.Services.PluginManager;
using Sharpbot.Services.PluginManager.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sharpbot.Plugins.Commands
{
    [PluginMeta("Commands","Plugin Handling the Recieveing, Interprition, and Execution of Text Commands")]
    [PPriority(1)]
    public class CommandPlugin : IPlugin
    {

        public Dictionary<string, Command> Commands { get; private set; }
        public Dictionary<string, CommandGroup> Groups { get; private set; }

        public CommandPlugin()
        {
            Groups = new Dictionary<string, CommandGroup>();
            Commands = new Dictionary<string, Command>();
            AttachToClients();
            CommandGroup com = new Fun();
            LoadCommands(com);
            com = new Debug();
            LoadCommands(com);
        }

        public void LoadCommands(CommandGroup groups)
        {
            var comms = groups.GetCommands();
            Groups.Add(groups.Name, groups);
            foreach(var com in comms)
            {
                Console.WriteLine(com.Names.First());
                foreach(var name in com.Names)
                {
                    Commands.Add(name.ToLower(), com);
                }
            }
        }

        private void AttachToClients()
        {
            ClientManager.OnMessageRequest += CommandScrub;
        }

        private Task CommandScrub(MessageRequest msg)
        {
            var priPrefix = "!";
            var secPrefix = "?";
            string mentionPrefix;
            CommandRequest comm = null;
            switch (msg.MessageMedium)
            {
                case Location.Guild:
                    mentionPrefix = "<@" + msg.DiscordBot.CurrentUser.Id + ">";
                    if (msg.Content.StartsWith(priPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(priPrefix.Length));

                    }
                    else if (msg.Content.StartsWith(secPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(priPrefix.Length));
                    }
                    else if (msg.Content.StartsWith(mentionPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(mentionPrefix.Length + 1));
                    }
                    break;
                case Location.DM:
                    mentionPrefix = "<@" + msg.DiscordBot.CurrentUser.Id + ">";
                    if (msg.Content.StartsWith(priPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(priPrefix.Length));

                    }
                    else if (msg.Content.StartsWith(secPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(priPrefix.Length));
                    }
                    else if (msg.Content.StartsWith(mentionPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(mentionPrefix.Length + 1));
                    }
                    break;
                case Location.Channel:
                    mentionPrefix = "@" + msg.TwitchBot.TwitchUsername;
                    if (msg.Content.StartsWith(priPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(priPrefix.Length));

                    }
                    else if (msg.Content.StartsWith(secPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(priPrefix.Length));
                    }
                    else if (msg.Content.ToLower().StartsWith(mentionPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(mentionPrefix.Length + 1));
                    }
                    break;
                case Location.Whisper:
                    mentionPrefix = "@" + msg.TwitchBot.TwitchUsername;
                    if (msg.Content.StartsWith(priPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(priPrefix.Length));

                    }
                    else if (msg.Content.StartsWith(secPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(priPrefix.Length));
                    }
                    else if (msg.Content.ToLower().StartsWith(mentionPrefix))
                    {
                        comm = new CommandRequest(msg, msg.Content.Substring(mentionPrefix.Length + 1));
                    }
                    break;
            }
            if (comm != null)
                RunCommand(comm);

            return Task.CompletedTask;
        }

        private async Task RunCommand(CommandRequest req)
        {
            Console.WriteLine("Attempting to run command");
            if (Commands.TryGetValue(req.CommandHead.ToLower(),out Command comm))
            {
                Console.WriteLine("Command Found -- " + req.CommandHead.ToLower());
                if(comm.IsProperUsage(req))
                    await comm.Excute(req);
            }
        }


    }
}
