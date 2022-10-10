using Discord.WebSocket;
using LevyBotSharp.Utility.Attributes;
using LevyBotSharp.Utility.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    public partial class Fun
    {

        [CommandMeta(new[] { "Hug" }, "Give other people Hugs!")]
        [GuildContext]
        public async Task HugCom(UserRequest req)
        {
            var guild = req.Guild;
            foreach (var arg in req.Args)
            {
                if (arg.AsString().ToLower() == "@everyone")
                {
                    if (req.Bot.CurrentUser.Id == 455194993993449482)
                    {
                        await req.Channel.SendMessageAsync("My arms aren't big enough :(");
                    }
                    else if (req.Bot.CurrentUser.Id == 319015824642015264)
                    {
                        await req.Channel.SendMessageAsync("I am amazed you have survied this long");
                    }
                    return;
                }
            }
            var check = req.Args.FirstOrDefault();
            SocketUser param;
            if (check != null)
            {
                param = await check.AsSocketUser(guild);
            }
            else
            {
                param = null;
            }
            if (param != null) //First Arg is a User
            {
                if (req.Bot.CurrentUser.Id == 455194993993449482) // Bot is Mocha
                {
                    if (param.Id == 455194993993449482) // Target is Mocha
                    {
                        await req.Channel.SendMessageAsync(
                            $"{req.Requester.Mention} gives a big hug to... ME!? THANK YOU SO MUCH! :purple_heart::purple_heart:");
                    }
                    else if (param.Id == 319015824642015264)
                    {
                        await req.Channel.SendMessageAsync(
                            $"Sorry, {req.Requester.Mention}! Levybot really does not like hugs 😦");
                    }
                    else if (param.Id == req.Requester.Id) //Self Hug
                    {
                        await req.Channel.SendMessageAsync(
                            $"Awww you don't need to hug yourself! I'll give you a hug {req.Requester.Mention}! :purple_heart:");
                    }
                    else
                    {
                        await HugDefaultCase(req);
                    }
                }
                else if (req.Bot.CurrentUser.Id == 319015824642015264) // Bot is Levybot
                {
                    if (param.Id == 319015824642015264) // Target is Levybot
                    {
                        await req.Channel.SendMessageAsync(
                        $"{req.Requester.Mention} tries to give a big hug to... me? Ew don't touch me with your filthy hands.");
                    }
                    else if (param.Id == 455194993993449482) // Target is Mocha
                    {
                        await req.Channel.SendMessageAsync(
                            $"Giving {param.Mention} a hug ey? She will probably enjoy that {req.Requester.Mention}.");
                    }
                    else if (param.Id == req.Requester.Id) //Self Hug
                    {
                        await req.Channel.SendMessageAsync(
                            $"Hugging yourself like a loner ey {req.Requester.Mention}? Is that even possible?");
                    }
                    else
                    {
                        await HugDefaultCase(req);
                    }
                }
                else
                {
                    await HugDefaultCase(req);
                }
            }
            else // First arg was not a user
            {
                await HugDefaultCase(req);
            }

        }

        private async Task HugDefaultCase(UserRequest req)
        {
            var target = req.Args.Merge();
            await req.Channel.SendMessageAsync($"{req.Requester.Mention} gives {target.AsString()} a big hug!");
        }


    }
}