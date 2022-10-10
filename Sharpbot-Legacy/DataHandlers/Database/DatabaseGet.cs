using Discord.WebSocket;
using LevyBotSharp.DataHandlers.Models;
using LevyBotSharp.Utility;
using LevyBotSharp.Utility.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace LevyBotSharp.DataHandlers.Database
{
    public static partial class DataHandler
    {

        public static async Task<int> GetGlobalPerms(ulong uId)
        {

            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT `perms` FROM `users` WHERE `id` = ?id;";
                    cmd.Parameters.AddWithValue("?id", uId);


                    var check = cmd.ExecuteScalar();
                    if (check == null)
                    {
                        await InitalizeUser(uId);
                        return 5;
                    }

                    var reader = cmd.ExecuteReader();

                    while (await reader.ReadAsync())
                        return reader.GetInt32("perms");

                    return 5;
                }
            }

        }

        public static async Task<GuildSettings> GetSettings(ulong guildId, ulong botId)
        {
            Console.WriteLine($"Getting {guildId}");
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM `guild_settings` WHERE `ID` = ?id;";
                    cmd.Parameters.AddWithValue("?id", DataUtil.ConstructId(guildId, botId));


                    var check = cmd.ExecuteScalar();
                    if (check == null)
                    {
                        Console.WriteLine($"Settings for {guildId} DNE");
                        await InitalizeGuild(guildId, botId);
                        return new GuildSettings(guildId);

                    }

                    var reader = cmd.ExecuteReader();
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"Settings for {guildId} Do Exist");
                        var sett = new GuildSettings
                        {
                            PriPrefix = reader.GetString("Prefix"),
                            GuildId = reader.GetUInt64("guild"),
                            ValidStars = (PermLevel)reader.GetUInt32("StarboardPerm"),
                            IsLogging = reader.GetBoolean("IsLogging"),
                            EnableStarboard = reader.GetBoolean("EnableStarboard")
                           
                        };
                        sett.initLogs();
                        if (!reader.IsDBNull(4))
                            sett.SecPrefix = reader.GetString("secPrefix");

                        sett.StarboardChannel = !reader.IsDBNull(6) ? reader.GetUInt64("Starboard") : 0;

                        sett.Logs[(int)LogType.Infraction] = !reader.IsDBNull(11) ? reader.GetUInt64("InfractionLog") : 0;
                        sett.EnableLog[(int)LogType.Infraction] = reader.GetBoolean("IsInfractionLog");

                        sett.Logs[(int)LogType.Delete] = !reader.IsDBNull(13) ? reader.GetUInt64("DeleteLog") : 0;
                        sett.EnableLog[(int)LogType.Delete] = reader.GetBoolean("IsDeleteLog");

                        sett.Logs[(int)LogType.Edit] = !reader.IsDBNull(15) ? reader.GetUInt64("EditLog") : 0;
                        sett.EnableLog[(int)LogType.Edit] = reader.GetBoolean("IsEditLog");

                        sett.Logs[(int)LogType.ChannelDel] = !reader.IsDBNull(17) ? reader.GetUInt64("ChannelDelLog") : 0;
                        sett.EnableLog[(int)LogType.ChannelDel] = reader.GetBoolean("IsChannelDelLog");

                        sett.Logs[(int)LogType.ChannelEdit] = !reader.IsDBNull(19) ? reader.GetUInt64("ChannelEditLog") : 0;
                        sett.EnableLog[(int)LogType.ChannelEdit] = reader.GetBoolean("IsChannelEditLog");

                        sett.Logs[(int)LogType.ChannelAdd] = !reader.IsDBNull(21) ? reader.GetUInt64("ChannelAddLog") : 0;
                        sett.EnableLog[(int)LogType.ChannelAdd] = reader.GetBoolean("IsChannelAddLog");

                        sett.Logs[(int)LogType.RoleDel] = !reader.IsDBNull(23) ? reader.GetUInt64("RoleDelLog") : 0;
                        sett.EnableLog[(int)LogType.RoleDel] = reader.GetBoolean("IsRoleDelLog");

                        sett.Logs[(int)LogType.RoleEdit] = !reader.IsDBNull(25) ? reader.GetUInt64("RoleEditLog") : 0;
                        sett.EnableLog[(int)LogType.RoleEdit] = reader.GetBoolean("IsRoleEditLog");

                        sett.Logs[(int)LogType.RoleAdd] = !reader.IsDBNull(27) ? reader.GetUInt64("RoleAddLog") : 0;
                        sett.EnableLog[(int)LogType.RoleAdd] = reader.GetBoolean("IsRoleAddLog");

                        sett.Logs[(int)LogType.UBan] = !reader.IsDBNull(29) ? reader.GetUInt64("UBanLog") : 0;
                        sett.EnableLog[(int)LogType.UBan] = reader.GetBoolean("IsUBanLog");

                        sett.Logs[(int)LogType.UJoin] = !reader.IsDBNull(31) ? reader.GetUInt64("UJoinLog") : 0;
                        sett.EnableLog[(int)LogType.UJoin] = reader.GetBoolean("IsUJoinLog");

                        sett.Logs[(int)LogType.ULeft] = !reader.IsDBNull(33) ? reader.GetUInt64("ULeftLog") : 0;
                        sett.EnableLog[(int)LogType.ULeft] = reader.GetBoolean("IsULeftLog");

                        sett.Logs[(int)LogType.UVoice] = !reader.IsDBNull(35) ? reader.GetUInt64("UVoiceLog") : 0;
                        sett.EnableLog[(int)LogType.UVoice] = reader.GetBoolean("IsUVoiceLog");

                        sett.Logs[(int)LogType.MemberEdit] = !reader.IsDBNull(37) ? reader.GetUInt64("MemEditLog") : 0;
                        sett.EnableLog[(int)LogType.MemberEdit] = reader.GetBoolean("IsMemEditLog");

                        sett.Logs[(int)LogType.GuildEdit] = !reader.IsDBNull(39) ? reader.GetUInt64("GuildEditLog") : 0;
                        sett.EnableLog[(int)LogType.GuildEdit] = reader.GetBoolean("IsGuildEditLog");

                        sett.Logs[(int)LogType.UUnban] = !reader.IsDBNull(41) ? reader.GetUInt64("UUnbanLog") : 0;
                        sett.EnableLog[(int)LogType.UUnban] = reader.GetBoolean("IsUUnbanLog");

                        sett.SetBotId(botId);

                        return sett;
                    }
                    return new GuildSettings(guildId);
                }
            }
        }

        public static async Task<PermLevel> GetGuildPerms(SocketGuildUser user)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT `perm` FROM `guild_members` WHERE `ID` = ?Id;";
                    cmd.Parameters.AddWithValue("?Id", DataUtil.ConstructId(user.Guild.Id, user.Id));

                    var check = cmd.ExecuteScalar();
                    if (check == null)
                    {
                        InitalizeGuildUser(user.Guild.Id, user.Id);
                        return PermLevel.Normal;
                    }
                    var reader = cmd.ExecuteReader();
                    while (await reader.ReadAsync())
                    {
                        return (PermLevel) reader.GetInt32("perm");
                    }
                    return PermLevel.Normal;
                }
            }
        }

        public static async Task<string> GetSocialPlat(ulong uId, SPlatform platId)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    switch (platId)
                    {
                        case SPlatform.Twitch:
                            cmd.CommandText = "SELECT `id`, `twitch` FROM `users` WHERE `id` = ?id;";
                            break;
                        case SPlatform.TwitchId:
                            cmd.CommandText = "SELECT `id`, `twitch_id` FROM `users` WHERE `id` = ?id";
                            break;
                        case SPlatform.Steam:
                            cmd.CommandText = "SELECT `id`, `steam` FROM `users` WHERE `id` = ?id";
                            break;
                        case SPlatform.SteamUrl:
                            cmd.CommandText = "SELECT `id`, `steam_url` FROM `users` WHERE `id` = ?id";
                            break;
                        case SPlatform.Twitter:
                            cmd.CommandText = "SELECT `id`, `twitter` FROM `users` WHERE `id` = ?id";
                            break;
                        case SPlatform.Switch:
                            cmd.CommandText = "SELECT `id`, `switch` FROM `users` WHERE `id` = ?id";
                            break;
                        case SPlatform.PSN:
                            cmd.CommandText = "SELECT `id`, `psn` FROM `users` WHERE `id` = ?id";
                            break;
                        case SPlatform.Xbox:
                            cmd.CommandText = "SELECT `id`, `xbox` FROM `users` WHERE `id` = ?id";
                            break;
                        default:
                            return null;
                    }


                    cmd.Parameters.AddWithValue("?id", uId);

                    var check = cmd.ExecuteScalar();
                    if (check == null)
                    {
                        await InitalizeUser(uId);
                        return null;
                    }

                    var reader = cmd.ExecuteReader();

                    while (await reader.ReadAsync())
                        return !reader.IsDBNull(1) ? reader.GetString(1) : string.Empty;

                    return null;

                }
            }
        }



        public static async Task<RoleInfo> GetRole(SocketRole role)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "SELECT * FROM `guild_roles` WHERE `role` = ?rId;";
                    cmd.Parameters.AddWithValue("?rId", role.Id);

                    var check = cmd.ExecuteScalar();
                    if (check == null)
                    {
                        await InitalizeRole(role.Guild.Id, role.Id);
                        return new RoleInfo
                        {
                            Id = role.Id,
                            Perms = PermLevel.Normal,
                            Lv = -1,
                            LvReward = false,
                            SelfAssign = false,
                            MuteRole = false

                        };
                    }

                    var reader = cmd.ExecuteReader();
                    while (await reader.ReadAsync())
                    {
                        return new RoleInfo
                        {
                            Id = role.Id,
                            Perms = (PermLevel)reader.GetInt32("perm"),
                            LvReward = reader.GetBoolean("IsRwd"),
                            Lv = reader.GetInt32("lvl"),
                            SelfAssign = reader.GetBoolean("self"),
                            MuteRole = reader.GetBoolean("IsMute")
                        };
                    }
                    return new RoleInfo
                    {
                        Id = role.Id,
                        Perms = PermLevel.Normal,
                        Lv = -1,
                        LvReward = false,
                        SelfAssign = false

                    };
                }
            }
        }
    }
}
