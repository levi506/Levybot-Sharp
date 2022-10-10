using Discord;
using LevyBotSharp.DataHandlers.Database;
using LevyBotSharp.DataHandlers.Pipeline;
using LevyBotSharp.Utility.Apis;
using LevyBotSharp.Utility.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    class Stream : CommandGroupBase
    {
        public override string Name { get; } = "Stream";

        [CommandMeta(new[] {"Twitch"}, "Gets a streamer and their status")]
        [GlobalContext]
        [PMContext]
        [GuildContext]
        public async Task TwitchCom(UserRequest req)
        {
            var username = req.Args.First().AsString();
            var user = await Twitch.GetUser(username);
            if (req.SpInstruct.ContainsKey("set"))
            {
                if (username != null && user != null)
                {
                    await DataHandler.SetSocial(req.Requester.Id, username, 0);
                    if(await DataHandler.ContainsTwitch(user.Id))
                    {
                        await DataHandler.SetSocial(req.Requester.Id, user.Id, 1);
                        await req.SendSimpleEmbedAsync($"Set Your Twitch Identity to {username}!", "Social Update");
                    }

                    await req.SendSimpleEmbedAsync(
                        $"Your Twitch Username is now set to {username}, but the Id is already taken\n",
                        "Social Update");

                }
                else
                {

                    await req.SendSimpleEmbedAsync($"Sorry that isn't valid...", "Social Update");
                }
            }
            else
            {
                var stream = await Twitch.GetStream(username);
                if(stream !=null)
                {
                    var embed = LivestreamPipe.MakeStreamEmbed(stream, user);
                    await req.Channel.SendMessageAsync(embed: embed);
                }
                else if(user != null)
                {
                    var embed = new EmbedBuilder()
                        .WithCurrentTimestamp()
                        .WithColor(100, 65, 165);
                    embed.WithAuthor(user.DisplayName, user.ProfileImageUrl, $"https://twitch.tv/{user.DisplayName}")
                        .WithDescription(user.Description)
                        .WithFooter($"Views: {user.ViewCount} • User Id: {user.Id}","http://www.stickpng.com/assets/images/580b57fcd9996e24bc43c540.png")
                        .WithImageUrl(user.OfflineImageUrl
                            .Replace("{height}", "540")
                            .Replace("{width}", "960"));
                    await req.Channel.SendMessageAsync(embed: embed.Build());
                } 
                
            }
        }
    }
}
