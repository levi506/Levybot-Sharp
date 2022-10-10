using Discord;
using Discord.WebSocket;
using LevyBotSharp.DataHandlers.Database;
using LevyBotSharp.Localization;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Attributes;
using LevyBotSharp.Utility.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    class Debug : CommandGroupBase
    {
        public override string Name { get; } = "Debug";


        [CommandMeta(new[] { "Shutdown" }, "Turns Off the bot")]
        [GlobalContext]
        public async Task Shutdown(UserRequest req)
        {
            if (req.Permissions == PermLevel.Owner)
            {
                Console.WriteLine($"Shutting Down Engaged on Request of {req.Requester}");
                await req.SourceMessage.Channel.SendMessageAsync("Global Perm Valid :: Shutdown Request Accepted");
                await Task.Delay(3600);
                //TODO :: Write Shutdown Procedures
                Environment.Exit(0);
            }
            else
            {
                await req.SourceMessage.Channel.SendMessageAsync(
                    $"{req.Requester} lacks the global permission level to initiate shutdown");
            }

        }

        [CommandMeta(new[] { "Ping" }, "Check's bot latency to the server")]
        [GlobalContext]
        [GuildContext]
        [PMContext]
        public async Task Ping(UserRequest req)
        {

            await req.SourceMessage.Channel.SendMessageAsync($"Pong! {req.Bot.Latency}");

        }

        [CommandMeta(new[] {"modTest"})]
        [GuildContext]
        public async Task uTest(UserRequest req)
        {
            var ulist = await req.Args.ListAsUsers();
            string users = string.Empty;
            var reason = req.SourceMessage.Content.Split(", ")[1];

            foreach (SocketUser user in ulist)
            {
                users += user + "\n";
            }

            await req.Channel.SendMessageAsync($"I found these users with that \n{users}");

        }

        [CommandMeta(new[] {"Languages"},"Grabs Langauges Loaded into runtime")]
        [GlobalContext]
        public async Task GetLanguages(UserRequest req)
        {
            if(req.Permissions >= PermLevel.Moderator)
            {
                var langs = await LangHandler.GetCodes();
                string Description = "";
                foreach(var lang in langs)
                {
                    Description += lang + "/n";
                }
                var embed = new EmbedBuilder()
                    .WithAuthor(req.Bot.CurrentUser)
                    .WithTitle("Loaded Langauges")
                    .WithDescription(Description);

                await req.Channel.SendMessageAsync(string.Empty, embed: embed.Build());
            } else
            {
                await req.Channel.SendMessageAsync("You lack the permissions to use this command");
            }
        }

        //[CommandMeta(new[] {"Uptime"}, "Gets how long the bot core has been running")]
        [GlobalContext]
        [GuildContext]
        [PMContext]
        public async Task GetUptime(UserRequest req)
        {
           // var timePassed = Program.Uptime.Elapsed;
          //  var message =
           //     $"The bot core has been running for {timePassed.Days} days, {timePassed.Hours} hours, {timePassed.Minutes} minutes, and {timePassed.Seconds} seconds";
          //  await req.SendSimpleEmbedAsync(message);
        }

        [CommandMeta(new[] {"idtest"})]
        [GuildContext]
        public async Task TestingIdUtil(UserRequest req)
        {
            await req.SendSimpleEmbedAsync(DataUtil.ConstructId(req.Requester.Id, req.GuildId));
        }


    }
}
