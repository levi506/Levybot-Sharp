using Discord;
using System;
using System.IO;
using System.Linq;

namespace LevyBotSharp.Utility.Helpers
{
    public static class MessageHelper
    {

        public static MessageLoggable MakeLoggable(IMessage msg)
        {
            var embed = new EmbedBuilder();
            embed.WithAuthor(msg.Author);
            embed.WithDescription(msg.Content);
            var first = msg.Attachments.FirstOrDefault();
            MemoryStream file = null;
            var ch = msg.Channel as ITextChannel;

            if (first != null && first.Size != 0 && (ch != null && !ch.IsNsfw))
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

            if(first != null && first.Size != 0 && (ch != null && ch.IsNsfw))
            {
                embed.AddField("Attachment", "NSFW Channel\nAttachment refrained");
            }

            embed.WithCurrentTimestamp();

            MessageLoggable toLog = new MessageLoggable
            {
                Embed = embed.Build(),
                File = file,
                Src = msg,
                HasFile = (first != null && first.Size != 0 && (first.Size < 8000000 && file?.Length < 1)),
                FileName = first?.Filename
            };

            return toLog;
        }

    }

    public struct MessageLoggable
    {
        public IMessage Src { get; set; }

        public Embed Embed { get; set; }

        public MemoryStream File { get; set; }

        public bool HasFile { get; set; }

        public string FileName { get; set; }

    }

}
