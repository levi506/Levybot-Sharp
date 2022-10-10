using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Helpers;

namespace LevyBotSharp.TwitchHandlers.Plugins.Commands
{
    public static class TwitchCommandHandler
    {
        private static Dictionary<string, Command> Commands { get; } = new Dictionary<string, Command>();

        public static void Build()
        {
            var commandContainers = typeof(TwitchCommandHandler).Assembly.GetTypes()
                .Where(x => typeof(CommandGroupBase).IsAssignableFrom(x) && !x.IsAbstract);

            foreach (var commandContainer in commandContainers)
            {
                if (!(Activator.CreateInstance(commandContainer) is CommandGroupBase obj))
                    continue;
                var commands = obj.GetCommands();
                foreach (var command in commands)
                {
                    foreach (var name in command.Names)
                    {
                        Commands.Add(name.ToLowerInvariant(), command);
                    }
                }

                Console.WriteLine(
                    $"Loaded following twitch commands for twitch container {obj.Name}: {commands.Select(x => x.Names.First()).Join(", ")}");

            }

        }

        public static async Task ExecuteCom(UserRequest uIn)
        {
            if (uIn.Command != null && Commands.ContainsKey(uIn.Command))
            {
                if (Commands.TryGetValue(uIn.Command, out Command com))
                {
                    switch (uIn.Area)
                    {
                        case Locale.Global:
                            if (!com.Global)
                                await com.Invoke(uIn);
                            break;
                        case Locale.Whisper:
                            if (!com.Whisper)
                                await com.Invoke(uIn);
                            break;
                        case Locale.Channel:
                            if (!com.Channel)
                                await com.Invoke(uIn);
                            break;
                    }


                }
            }

        }
    }
}
