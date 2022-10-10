using Discord.WebSocket;
using LevyBotSharp.DataHandlers.Database;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LevyBotSharp.DataHandlers.Models
{
    public class GuildData : IData
    {
        public DiscordSocketClient Bot { get; private set; }

        public ulong GuildId { get; }

        public ISettings SettCache { get; set; }

        public RoleInfo MuteRole { get; private set; }

        public Dictionary<ulong, RoleInfo> RoleDataCache { get; private set; }

        public GuildData(DiscordSocketClient bot, SocketGuild guild)
        {
            Bot = bot;

            GuildId = guild.Id;

            RoleDataCache = new Dictionary<ulong, RoleInfo>();

            CompileRoles();

            CompileSettings();

        }

        public void CompileSettings()
        {
            SettCache = DataHandler.GetSettings(GuildId, Bot.CurrentUser.Id).GetAwaiter().GetResult();
        }

        public async Task CompileRoles()
        {
            RoleDataCache.Clear();
            var g = Bot.GetGuild(GuildId);
            var roles = g.Roles;
            foreach (var role in roles)
            {
                var roleData = await DataHandler.GetRole(role);
                if (roleData.MuteRole == true)
                    MuteRole = roleData;
                RoleDataCache.Add(role.Id,roleData);
            }
        }

        public async Task<PermLevel> ResolvePerm(SocketGuildUser user)
        {
            await CompileRoles();
            if (user.Guild.OwnerId == user.Id || Program.GetOwnerId() == user.Id) return PermLevel.Owner;

            var perms = new List<PermLevel>();

            foreach (var role in user.Roles)
            {
                if (RoleDataCache.TryGetValue(role.Id, out var data))
                    perms.Add(data.Perms);
                else
                    await DataHandler.InitalizeRole(role.Guild.Id, role.Id);
            }

            perms.Add(await DataHandler.GetGuildPerms(user));

            var followThrough = PermLevel.Normal;

            foreach (var perm in perms)
            {
                if (followThrough == PermLevel.Normal)
                {
                    followThrough = perm;
                }
                else if (followThrough < PermLevel.Normal)
                {
                    if (perm > PermLevel.Regular)
                    {
                        followThrough = perm;
                    }
                    else if (perm == PermLevel.Banned)
                    {
                        followThrough = perm;
                    }
                }
                else if (followThrough == PermLevel.Regular && perm != PermLevel.Normal)
                {
                    followThrough = perm;
                }
                else if (followThrough > PermLevel.Regular && perm > followThrough)

                {
                    followThrough = perm;
                }
            }

            return followThrough;
        }

        public SocketGuildChannel GetLog(LogType type)
        {
            return SettCache.GetLog(type);
        }

        public bool GetLogCheck(LogType type)
        {
            return SettCache.GetLogCheck(type);
        }
    }

    public struct RoleInfo
    {
        public ulong Id { get; set; }
        public PermLevel Perms { get; set; }
        public bool LvReward { get; set; }
        public int Lv { get; set; }
        public bool SelfAssign { get; set; }
        public bool MuteRole { get; set; }
    }
}
