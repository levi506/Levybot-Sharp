using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Attributes;
using LevyBotSharp.DiscordHandlers.Plugins.Moderation;
using LevyBotSharp.Utility.Helpers;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    public partial class Moderation : CommandGroupBase
    {
        public override string Name { get; } = "Moderation";

        //[CommandMeta(new[] { "Warn" }, "Adds a warning infraction and messages the user", "Moderation")]
        [GuildContext]
        public async Task Warn(UserRequest req)
        {
            if (req.Permissions < PermLevel.Helper) return;
            var user = await req.Args.First().AsSocketUser(req.Guild, true);
            req.Args.RemoveAt(0);
            var reason = req.Args.Merge().AsString();
            var guild = req.Guild;
            var embed = new EmbedBuilder()
                .WithAuthor(req.Requester)
                .WithTitle($"You were warned in {guild.Name}")
                .WithDescription($"{reason}")
                .WithCurrentTimestamp()
                .Build();
            await InfractionHandler.CreateWarn(user.Id, req.SourceMessage.Id, reason, req.Requester.Id,guild.Id);
            try
            {
                await user.SendMessageAsync(string.Empty, embed: embed);
                await req.Channel.SendMessageAsync($"{user} got message");
            }
            catch (HttpException)
            {
                await req.Channel.SendMessageAsync($"{user} didn't get message");
            }

        }


        //[CommandMeta(new[] { "Ban" }, "Bans a User", "Moderation")]
        [GuildContext]
        public async Task SBanCom(UserRequest req)
        {
            switch (req.Area)
            {
                case Locale.Guild:
                    if (req.Permissions < PermLevel.Moderator) return;
                    var user = await req.Args.First().AsSocketUser(req.Guild, true);
                    req.Args.RemoveAt(0);
                    var reason = req.Args.Merge().AsString();
                    var guild = req.Guild;
                    await InfractionHandler.CreateBan(user.Id, req.SourceMessage.Id, reason, req.Requester.Id,guild.Id);
                        var logEmbed = new EmbedBuilder()
                            .WithAuthor(req.Requester)
                            .WithTitle("A User was Banned!")
                            .WithDescription($"{reason}")
                            .WithCurrentTimestamp();
                        //var log = (ISocketMessageChannel)await req.Settings.GetInfractLog(req.Bot);
                        //await log.SendMessageAsync(string.Empty, embed: logEmbed.Build());
                    break;
                case Locale.Global:

                    break;
            }
        }

        //[CommandMeta(new[] { "Kick" }, "Kicks a User", "Moderation")]
        [GuildContext]
        public async Task SKickCom(UserRequest req)
        {
            if (req.Permissions < PermLevel.Moderator) return;
            var user = await req.Args.First().AsSocketUser(req.Guild, true);
            req.Args.RemoveAt(0);
            var reason = req.Args.Merge().AsString();
            var guild = req.Guild;
            await guild.GetUser(user.Id).KickAsync();
            await InfractionHandler.CreateKick(user.Id, req.SourceMessage.Id, reason, req.Requester.Id, guild.Id);
            
        }

        //[CommandMeta(new[] { "Mute" }, "Mutes a User","Moderation")]
        [GuildContext]
        public async Task SMute(UserRequest req)
        {
            if (req.Permissions < PermLevel.Helper) return;
            var user = await req.Args.First().AsSocketUser(req.Guild, true);
            req.Args.RemoveAt(0);
            var reason = req.Args.Merge().AsString();
            var guild = req.Guild;
            await InfractionHandler.CreateWarn(user.Id, req.SourceMessage.Id, reason, req.Requester.Id, guild.Id);
        }



        //[CommandMeta(new[] { "Clean", "Purge" }, "Clears Messages With Criteria", "Moderation")]
        [GuildContext]
        public async Task Clean(UserRequest req)
        {

        }

        //[CommandMeta(new[] { "ForceBan" }, "Forces a ban based on id","Moderation")]
        [GlobalContext]
        [GuildContext]
        public async Task FBan(UserRequest req)
        {

            var user = req.Args.First().AsUlong().HasValue ? req.Args.First().AsUlong().Value : 0;
            if (req.Permissions < PermLevel.Admin && user == 0) return;
            req.Args.RemoveAt(0);
            var reason = req.SourceMessage.Content.Split(", ")[1];
            var guild = req.Bot.GetGuild(req.GuildId);

            await InfractionHandler.CreateBan(user, req.SourceMessage.Id, reason, req.Requester.Id, guild.Id);
            await guild.AddBanAsync(userId: user, reason: reason);
        }

    }
}
