using Discord;
using LevyBotSharp.Utility.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    public class Info : CommandGroupBase
    {
        public override string Name { get; } = "Info";
        private const string OtherStr = "[@AveryIsabellArt](https://twitter.com/AveryIsbellArt) - Made the LevyBot's Icon three times\n[@Nicoohai](https://twitter.com/nicoohai) - Made Mocha's Icon";

        [CommandMeta(new[] { "Info" }, "Displays info about the bot")]
        [GlobalContext]
        [GuildContext]
        [PMContext]
        public async Task InfoCommand(UserRequest req)
        {
            var embed = new EmbedBuilder();

            embed.WithAuthor(req.Bot.CurrentUser);
            embed.WithDescription("General Use Discord Bot written in C#");
            embed.AddField("Library", "[Discord.Net](https://github.com/RogueException/Discord.Net)", true);
            embed.AddField("Creator", "[Levi506#2895](https://twitter.com/levi506lps)", true);
            embed.AddField("Servers", req.Bot.Guilds.Count, true);
            embed.AddField("Other Attributions", OtherStr,false);

            await req.Channel.SendMessageAsync(string.Empty, embed: embed.Build());
        }

        [CommandMeta(new[] { "Server" }, "Returns an embed of a server's info")]
        [GuildContext]
        public async Task ServerCom(UserRequest req)
        {
            var guild = req.Guild;

            await guild.DownloadUsersAsync();

            //Getting User Status Accounts
            var onUser = guild.Users.Count(x => x.Status == UserStatus.Online && !x.IsBot);
            var idUser = guild.Users.Count(x => (x.Status == UserStatus.Idle || x.Status == UserStatus.AFK) && !x.IsBot);
            var dndUser = guild.Users.Count(x => x.Status == UserStatus.DoNotDisturb && !x.IsBot);
            var offlineUser = guild.Users.Count(x => (x.Status == UserStatus.Invisible || x.Status == UserStatus.Offline) && !x.IsBot);
            var users = guild.Users.Count(x => !x.IsBot);
            var bots = guild.Users.Count(x => x.IsBot);

            var statStr = $"<:Online:364507703470456836> : {onUser}\n<:Idle:364507703336239105> : {idUser}\n" +
                          $"<:Dnd:364507703705468928> : {dndUser}\n<:Invis:364507703693017088> : {offlineUser}\n<:AllStat:372236378290913292> : {users}";

            var general = $"**Owner** : {guild.Owner}\n**Channels** : {guild.Channels.Count}\n**Emotes** : {guild.Emotes.Count}\n**Created** : {guild.CreatedAt.UtcDateTime}\n**Bots** : {bots}";

            //Embed Construction
            var embed = new EmbedBuilder();
            embed.AddField("General Info", general);
            embed.AddField("Users", statStr);
            embed.WithAuthor(guild.Name);
            embed.WithThumbnailUrl(guild.IconUrl);
            embed.WithCurrentTimestamp();


            await req.Channel.SendMessageAsync(string.Empty, embed: embed.Build());
        }

        [CommandMeta(new[] { "Prefix" }, "Returns the server's Command Prefix")]
        [GlobalContext]
        [GuildContext]
        [PMContext]
        public async Task PrefixCom(UserRequest req)
        {
            var prim = req.Settings.SettCache.PriPrefix;
            var sec = req.Settings.SettCache.SecPrefix;

            await req.Channel.SendMessageAsync($"The prefix for this location is `{prim}` !");
        }

    }
}
