using LevyBotSharp.DataHandlers.Database;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Attributes;
using LevyBotSharp.Utility.Interfaces;
using System.Threading.Tasks;

namespace LevyBotSharp.DiscordHandlers.Plugins.Commands.Groups
{
    class Settings : CommandGroupBase
    {
        public override string Name { get; } = "Settings";

        [CommandMeta(new[] {"Logs"},"Setting which channels will be used for logging","Settings")]
        [GuildContext]
        public async Task LogSettings(UserRequest req)
        {
            if (req.Permissions <= PermLevel.Admin)
            {
                await ToggleLog(req, "infract", LogType.Infraction);
                await ToggleLog(req, "ban", LogType.UBan);
                await ToggleLog(req, "join", LogType.UJoin);
                await ToggleLog(req, "leave", LogType.ULeft);
                await ToggleLog(req, "msgdel", LogType.Delete);
                await ToggleLog(req, "msgedit", LogType.Edit);
                await ToggleLog(req, "channeldel", LogType.ChannelDel);
                await ToggleLog(req, "channeladd", LogType.ChannelAdd);
                await ToggleLog(req, "channeledit", LogType.ChannelEdit);
                await ToggleLog(req, "roleadd", LogType.RoleAdd);
                await ToggleLog(req, "roledel", LogType.RoleDel);
                await ToggleLog(req, "uservoice", LogType.UVoice);
                await ToggleLog(req, "roleedit", LogType.RoleEdit);
                await ToggleLog(req, "guildedit", LogType.GuildEdit);
                await ToggleLog(req, "userunban", LogType.UUnban);
                await ToggleLog(req, "memberedit", LogType.MemberEdit);
            } else
            {
                return;
            }
        }

        private async Task ToggleLog(UserRequest req, string key, LogType type)
        {
            var SettId = DataUtil.ConstructId(req.GuildId, req.Bot.CurrentUser.Id);
            if (req.SpInstruct.ContainsKey(key))
            {
                var isOn = req.Settings.SettCache.EnableLog[(int)type];
                if (req.SpInstruct.TryGetValue(key, out var instruct))
                {
                    if (instruct.Variance.AsUlong().HasValue && req.Guild.GetTextChannel(instruct.Variance.AsUlong().Value) != null)
                    {
                        req.Settings.SettCache.EnableLog[(int)LogType.RoleEdit] = true;
                        await DataHandler.ToggleLog(SettId, true, type);
                        await DataHandler.SetLog(SettId, instruct.Variance.AsUlong().Value + "", type);
                    }
                    else
                    {
                        await DataHandler.ToggleLog(SettId, !isOn, type);
                        req.Settings.SettCache.EnableLog[(int)type] = !isOn;
                    }
                }
            }
        }
    }
}
