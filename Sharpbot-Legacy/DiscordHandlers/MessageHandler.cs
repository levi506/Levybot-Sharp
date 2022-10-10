using Discord;
using Discord.WebSocket;
using LevyBotSharp.DataHandlers;
using LevyBotSharp.DiscordHandlers.Plugins.Commands;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Helpers;
using LevyBotSharp.Utility.Interfaces;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers
{

    public static partial class MainHandler
    {

        public static async Task HandleMessage(SocketMessage sockMsg, DiscordSocketClient b)
        {
                var msg = sockMsg as SocketUserMessage;
                if (msg?.Author.IsBot ?? true)
                {
                    return;
                }

                UserRequest uReq;
                if (msg.Channel is SocketGuildChannel channel)
                {
                    var id = new Identifier { BotId = b.CurrentUser.Id, GuildId = channel.Guild.Id };
                    var settings = GuildCollection.RequestServer(id);
                    uReq = await UserRequest.ReqBuilder(msg, b, settings);
                }
                else
                {
                    LogDm(msg, b);
                    uReq = await UserRequest.ReqBuilder(msg, b, GuildCollection.GetGlobal(b.CurrentUser.Id));
                }

                if (uReq?.Command != null)
                {
                    if (uReq.Permissions != PermLevel.Banned)
                        CommandHandler.ExecuteCom(uReq);
                }

        }

        public static async Task LogDm(SocketUserMessage msg, DiscordSocketClient bot)
        {

            var log = (ISocketMessageChannel)GuildCollection.GetGlobal(bot.CurrentUser.Id).GetLog(LogType.Edit);

            var loggable = MessageHelper.MakeLoggable(msg);

            if (!loggable.HasFile)
            {
                await log.SendMessageAsync(string.Empty, embed: loggable.Embed);
            }
            else
            {
                await log.SendFileAsync(loggable.File, loggable.FileName, embed: loggable.Embed);
            }

        }

        public static async Task MessageDeleted(Cacheable<IMessage, ulong> msgCache, ISocketMessageChannel channel,
            DiscordSocketClient client)
        {
            var guildChannel = channel as IGuildChannel;
            if (guildChannel != null)
            {
                var identifier = new Identifier
                {
                    BotId = client.CurrentUser.Id,
                    GuildId = guildChannel.GuildId
                };
                var msg = msgCache.Value;
                var serverData = GuildCollection.RequestServer(identifier);
                var log = serverData.GetLog(LogType.Delete) as ISocketMessageChannel;
                var enableLog = serverData.GetLogCheck(LogType.Delete);

                if (enableLog && msg != null && log != null)
                {
                    var loggable = MessageHelper.MakeLoggable(msg);

                    if (!loggable.HasFile)
                    {
                        await log.SendMessageAsync(string.Empty, embed: loggable.Embed);
                    }
                    else
                    {
                        await log.SendFileAsync(loggable.File, loggable.FileName, embed: loggable.Embed);
                    }

                }


            }
        }

    }
}