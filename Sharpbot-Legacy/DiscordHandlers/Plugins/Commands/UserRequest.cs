using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using LevyBotSharp.DataHandlers.Models;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Command;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands
{
    public enum Locale : sbyte
    {
        Global = 100,
        Dm = 50,
        Guild = 0
    }

    public class UserRequest
    {

        public SocketUser Requester { get; private set; }
        public SocketUserMessage SourceMessage { get; private set; }
        public ISocketMessageChannel Channel { get; private set; }
        public IData Settings { get; private set; }
        public SocketGuild Guild { get; private set; }
        public DiscordSocketClient Bot { get; private set; }
        public string Command { get; private set; }
        public List<Parameter> Args { get; private set; }
        public Dictionary<string, Instruction> SpInstruct { get; private set; }
        public PermLevel Permissions { get; private set; }
        public ulong GuildId { get; private set; }
        public Locale Area { get; private set; }
        public bool IsSecondary { get; private set; }


        public UserRequest(SocketUserMessage msg, DiscordSocketClient bot, IData data)
        {
            Requester = msg.Author;
            Bot = bot;
            SourceMessage = msg;
            Channel = msg.Channel;
            Args = new List<Parameter>();
            SpInstruct = new Dictionary<string, Instruction>();
            var content = msg.Content;
            Settings = data;

            if (!(msg.Channel is SocketGuildChannel))
            {
                Area = Locale.Dm;
                Guild = null;
            }
            else
            {
                var ch = (SocketGuildChannel)msg.Channel;
                Guild = ch.Guild;
                Area = Locale.Guild;

            }

            if (content.StartsWith('g'))
            {
                Area = Locale.Global;
                content = content.Substring(1);
            }

            GuildId = data.GuildId;

            content = PrefixCheck(content, bot, data);

            if (content == null)
            {
                Command = null;
                return;
            }

            if (content.StartsWith("GNv*MrT6 "))
            {
                IsSecondary = true;
                content = content.Substring(10);
            }

            var args = content.Split(" ");
            Command = args[0].ToLower();
            for (var index = 1; index < args.Length; index++)
            {
                var arg = args[index];
                if (arg.StartsWith("-"))
                {
                    var instruction = new Instruction(arg, bot);
                    SpInstruct.Add(instruction.Header, instruction);
                }
                else
                {
                    if (arg == string.Empty)
                        continue;
                    Args.Add(new Parameter(arg, Bot, Settings.GuildId));
                }
            }

        }

        public static async Task<UserRequest> ReqBuilder(SocketUserMessage u, DiscordSocketClient b, IData s)
        {
            var req = new UserRequest(u, b, s);
            if (req.Area > Locale.Guild)
            {
                req.Permissions = await PermissionsHandler.ResolveGlobalPerm(req.Requester);
            }
            else
            {
                var set = s as GuildData;
                req.Permissions = await set.ResolvePerm(req.Guild.GetUser(u.Author.Id));
            }

            return req;
        }

        private static string PrefixCheck(string content, DiscordSocketClient b, IData data)
        {
            if (content.StartsWith(data.SettCache.PriPrefix))
            {
                return content.Substring(data.SettCache.PriPrefix.Length);
            }

            if (data.SettCache.SecPrefix != null && string.IsNullOrEmpty(data.SettCache.SecPrefix) && content.StartsWith(data.SettCache.SecPrefix))
            {
                return "GNv*MrT6 " + content.Substring(data.SettCache.SecPrefix.Length);
            }

            var botMention = b.CurrentUser.Mention;

            if (content.StartsWith(botMention))
            {
                return content.Substring(botMention.Length + 1);
            }

            botMention = botMention.Substring(0, 2) + botMention.Substring(3);


            return content.StartsWith(botMention) ? content.Substring(botMention.Length + 1) : null;
        }

        public async Task<RestUserMessage> SendSimpleEmbedAsync(string message, string title = null)
        {
            var botname = Bot.CurrentUser.Username;
            var botAvi = Bot.CurrentUser.GetAvatarUrl();
            var embed = new EmbedBuilder()
                .WithAuthor(botname, botAvi)
                .WithDescription(message)
                .WithCurrentTimestamp()
                .WithColor(131, 150, 255)
                .WithFooter(Program.GetVersion(), ClientController.GetOwnerDiscord().GetAvatarUrl());
            if (title?.Length > 0)
            {
                embed.WithTitle(title);
            }

            return await Channel.SendMessageAsync(embed: embed.Build());
        }

        public SocketGuildUser GetMember()
        {
            return Guild?.GetUser(Requester.Id);
        }

        public SocketGuildUser GetBotGuild()
        {
            return Guild?.GetUser(Bot.CurrentUser.Id);
        }

        public SocketCommandContext GetContext()
        {
            return new SocketCommandContext(Bot, SourceMessage);
        }


    }
}