using Discord;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Attributes;
using LevyBotSharp.Utility.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    public partial class Fun
    {
        internal static Random Rand = new Random();

        [CommandMeta(new[] { "Jumoji" }, "Makes Emotes Huuuuuuuge")]
        [GlobalContext]
        [GuildContext]
        [PMContext]
        public async Task JumojiCom(UserRequest req)
        {

            if(Emote.TryParse(req.Args.First().AsString(), out var emote))
            {
                var msg = await req.Channel.SendMessageAsync("Ok, I'll start working!");
                int scale;
                if (!(req.SpInstruct.ContainsKey("size") && req.SpInstruct.TryGetValue("size", out var size) && size.Variance.AsInt().HasValue))
                {
                    scale = 256;
                }
                else
                {
                    scale = Math.Min(size.Variance.AsInt().Value,512);
                }
                if (!emote.Animated) { 
                    var jumoji = FileUtil.EffResizeImage(emote.Url, scale, scale);
                    var embed = new EmbedBuilder()
                          .WithAuthor(req.Bot.CurrentUser)
                          .WithTitle(emote.Name)
                          .WithImageUrl($"attachment://jumoji.png")
                          .Build();
                    jumoji.Position = 0;

                    await req.Channel.SendFileAsync(jumoji, "jumoji.png", embed: embed);
                    jumoji.Dispose();
                }
                else
                {
                    var image = emote.Url;
                    var jumoji = FileUtil.ResizeGif(image, scale, scale);
                    var embed = new EmbedBuilder()
                        .WithAuthor(req.Bot.CurrentUser)
                        .WithTitle(emote.Name)
                        .WithImageUrl($"attachment://jumoji.gif")
                        .Build();
                    jumoji.Position = 0;

                    await msg.DeleteAsync();
                    await req.Channel.SendFileAsync(jumoji, "jumoji.gif", embed: embed);
                    jumoji.Dispose();
                }
            }
        }

        [CommandMeta(new[] {"Choose", "Pick"}, "Selects and option from a list")]
        [GlobalContext]
        [GuildContext]
        [PMContext]
        public async Task ChooseCom(UserRequest req)
        {
            var arg = req.Args.Merge().AsString().Split(',');
            var num = Rand.Next(0, arg.Length);
            await req.SendSimpleEmbedAsync($"{arg[num]}!", "Eenie Meenie Miney Moe, I choose....");
        }
    }
}
