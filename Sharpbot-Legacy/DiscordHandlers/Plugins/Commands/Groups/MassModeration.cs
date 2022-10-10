using Discord;
using Discord.Net;
using LevyBotSharp.DiscordHandlers.Plugins.Moderation;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Attributes;
using System.Threading.Tasks;
using LevyBotSharp.Utility.Helpers;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    public partial class Moderation
    {

        //[CommandMeta(new[] { "mBan" }, "Bans multiple users from the Server")]
        [GlobalContext]
        [GuildContext]
        public async Task Ban(UserRequest req)
        {
            switch (req.Area)
            {
                case Locale.Guild:
                {
                    if (req.Permissions < PermLevel.Admin) return;
                    var ulist = await req.Args.ListAsUsers(true);
                    var reason = req.SourceMessage.Content.Split(", ")[1];
                    var guild = req.Guild;

                    foreach (var user in ulist)
                    {
                        await InfractionHandler.CreateBan(user.Id, req.SourceMessage.Id, reason, req.Requester.Id, guild.Id);
                        await guild.AddBanAsync(user, reason: reason);
                    }
                    break;
                }
                case Locale.Global:
                {

                    break;
                }
            }

        }

        //[CommandMeta(new[] { "mKick" }, "Kicks multiple users from the server")]
        [GuildContext]
        public async Task Kick(UserRequest req)
        {
            if (req.Permissions < PermLevel.Admin) return;
            var ulist = await req.Args.ListAsUsers(true);
            var reason = req.SourceMessage.Content.Split(", ")[1];
            var guild = req.Guild;

            foreach (var user in ulist)
            {
                await InfractionHandler.CreateKick(user.Id, req.SourceMessage.Id, reason, req.Requester.Id, guild.Id);
                await guild.GetUser(user.Id).KickAsync(reason);
            }
        }

        //[CommandMeta(new[] { "mWarn" }, "Adds a warning infraction and messages the users")]
        [GuildContext]
        public async Task MWarn(UserRequest req)
        {
            if (req.Permissions < PermLevel.Moderator) return;
            var ulist = await req.Args.ListAsUsers(true);
            var reason = req.SourceMessage.Content.Split(", ")[1];
            var guild = req.Guild;
            var embed = new EmbedBuilder()
                .WithAuthor(req.Requester)
                .WithTitle($"You were warned in {req.Guild.Name}")
                .WithDescription($"{reason}")
                .WithCurrentTimestamp()
                .Build();

            foreach (var user in ulist)
            {
                if (user.IsBot || user.IsWebhook) continue;
                await InfractionHandler.CreateWarn(user.Id, req.SourceMessage.Id, reason, req.Requester.Id, guild.Id);
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
        }

        //[CommandMeta(new[] { "mForceBan" }, "Bans a list of users not present in the server")]
        [GuildContext]
        [GlobalContext]
        public async Task ForceBan(UserRequest req)
        {
            if (req.Permissions < PermLevel.Admin) return;
            var ulist = req.Args.ListAsGlobalUsers();
            var reason = req.SourceMessage.Content.Split(", ")[1];
            var guild = req.Bot.GetGuild(req.GuildId);

            foreach (var user in ulist)
            {
                await InfractionHandler.CreateBan(user.Id, req.SourceMessage.Id, reason, req.Requester.Id, guild.Id);
                await guild.AddBanAsync(user, reason: reason);
            }
        }

    }
}
