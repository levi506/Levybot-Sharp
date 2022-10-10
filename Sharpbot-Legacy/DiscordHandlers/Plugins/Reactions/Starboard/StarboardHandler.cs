using System;
using System.IO;
using System.Linq;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using LevyBotSharp.DataHandlers;
using LevyBotSharp.Utility;

namespace LevyBotSharp.DiscordHandlers.Plugins.Reactions.Starboard
{
    public static class StarboardHandler
    {

        public static Emoji Star = new Emoji("⭐");
        public static Emoji NightSky = new Emoji("🌃");

        public static async void CheckStars(ReactionRequest req)
        {

            if (!req.SourceMessage.Reactions.TryGetValue(Star, out var reactMeta)) return;
            if (reactMeta.ReactionCount != 1 || !req.Settings.SettCache.EnableStarboard || !(req.Settings.SettCache.ValidStars >= req.perm)) return;

            var channel = (ISocketMessageChannel) req.Settings.SettCache.GetStarboard();
            var author = req.SourceMessage.Author;
            var embed = new EmbedBuilder()
                .WithAuthor(author.Username, author.GetAvatarUrl())
                .WithDescription(req.SourceMessage.Content)
                .WithFooter($"{req.SourceMessage.Id}")
                .AddField("Message", $"[Jump!](https://canary.discordapp.com/channels/{req.GuildId}/{req.Channel.Id}/{req.SourceMessage.Id})")
                .WithTimestamp(req.SourceMessage.Timestamp)
                .WithColor(252,232,17);
            var first = req.SourceMessage.Attachments.FirstOrDefault();
            MemoryStream file = null;

            if (first != null && first.Size != 0)
            {
                if (first.Size < 8000000)
                {
                    file = FileUtil.GetFile(first.Url);
                    if (FileUtil.LinkIsImage(first.Url))
                        embed.WithImageUrl($"attachment://{first.Filename}");
                }
                else
                {
                    if (FileUtil.LinkIsImage(first.Url))
                        embed.WithImageUrl($"{first.Url}");
                }
            }
            embed.WithCurrentTimestamp();
            RestUserMessage msg;

            if (first == null || first.Size == 0 || (first.Size < 8000000 && file?.Length < 1))
            {
                msg = await channel.SendMessageAsync(string.Empty, embed: embed.Build());
            }
            else
            {
                string fileName = first?.Filename;
                msg = await channel.SendFileAsync(file, fileName, embed: embed.Build());
            }


        }

        public static async void ModStar(ReactionRequest req)
        {
            if (!req.Settings.SettCache.EnableStarboard || !(req.perm <= PermLevel.Moderator)) return;
            var channel = (ISocketMessageChannel) req.Settings.SettCache.GetStarboard();
            var author = req.SourceMessage.Author;
            var embed = new EmbedBuilder()
                .WithAuthor(author.Username, author.GetAvatarUrl())
                .WithDescription(req.SourceMessage.Content)
                .WithFooter($"{req.SourceMessage.Id}")
                .AddField("Message", $"[Jump!](https://canary.discordapp.com/channels/{req.GuildId}/{req.Channel.Id}/{req.SourceMessage.Id})")
                .WithTimestamp(req.SourceMessage.Timestamp)
                .WithColor(0, 166, 216);
            var first = req.SourceMessage.Attachments.FirstOrDefault();
            MemoryStream file = null;

            if (first != null && first.Size != 0)
            {
                if (first.Size < 8000000)
                {
                    file = FileUtil.GetFile(first.Url);
                    if (FileUtil.LinkIsImage(first.Url))
                        embed.WithImageUrl($"attachment://{first.Filename}");
                }
                else
                {
                    if (FileUtil.LinkIsImage(first.Url))
                        embed.WithImageUrl($"{first.Url}");
                }
            }
            embed.WithCurrentTimestamp();

            if (first == null || first.Size == 0 || (first.Size < 8000000 && file?.Length < 1))
            {
                await channel.SendMessageAsync($"🌃 Mod Starred by {req.Requester.Username}", embed: embed.Build());
            }
            else
            {
                string fileName = first?.Filename;
                await channel.SendFileAsync(file, fileName,text: $"🌃 Mod Starred by {req.Requester.Username}", embed: embed.Build());
            }
        }

        public static async void CheckGlobalStars(ReactionRequest req)
        {

            var sett = GuildCollection.GetGlobal(req.Bot.CurrentUser.Id);
            var reactMeta = req.Meta;
            if (reactMeta.ReactionCount != 3) return;

            var channel = (ISocketMessageChannel) sett.SettCache.GetStarboard();
            var embed = new EmbedBuilder()
                .WithAuthor(req.SourceMessage.Author)
                .WithDescription(req.SourceMessage.Content)
                .WithFooter($"{req.SourceMessage.Id}")
                .WithTimestamp(req.SourceMessage.Timestamp)
                .AddField("Guild",req.Guild.Name,true)
                .AddField("Message",
                    $"[Jump!](https://canary.discordapp.com/channels/{req.GuildId}/{req.Channel.Id}/{req.SourceMessage.Id})");

            var first = req.SourceMessage.Attachments.FirstOrDefault();
            MemoryStream file = null;

            if (first != null && first.Size != 0)
            {
                if (first.Size < 8000000)
                {
                    Console.WriteLine(first.Url);
                    file = FileUtil.GetFile(first.Url);
                    if (FileUtil.LinkIsImage(first.Url))
                        embed.WithImageUrl($"attachment://{first.Filename}");
                    embed.AddField("Attachment", $"Attached");
                }
                else
                {
                    if (FileUtil.LinkIsImage(first.Url))
                        embed.WithImageUrl($"{first.Url}");
                    embed.AddField("Attachment", $"{first.Url}");
                }
            }

            if (first == null || first.Size != 0 || (first.Size < 8000000 && file?.Length < 1))
            {
                await channel.SendMessageAsync(string.Empty, embed: embed.Build());
            }
            else
            {
                string fileName = first?.Filename;
                await channel.SendFileAsync(file, fileName, embed: embed.Build());
            }
        }

    }
}
