using LevyBotSharp.DiscordHandlers.Plugins.Moderation;
using LevyBotSharp.Utility.Interfaces;
using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;

namespace LevyBotSharp.DataHandlers.Database
{
    public enum SPlatform
    {
        Twitch,
        TwitchId,
        Steam,
        SteamUrl,
        Twitter,
        Switch,
        PSN,
        Xbox
    }
    public partial class DataHandler
    {

        public static Task CreateInfraction(Infraction infraction)
        {
            using (var conn = new DatabaseConn())
            {

                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO `infractions` (`ID`, `reason`, `action`, `mod`, `guild`, `user`,`message`) VALUES (?id, ?reason, ?actionId, ?modId, ?guildId, ?userId, ?messageId);`";
                    //TODO :: Create Infraction Temp for SQL injection

                    cmd.Parameters.AddWithValue("?id", DataUtil.ConstructId(infraction.ServerId,infraction.MessageId));
                    cmd.Parameters.AddWithValue("?reason", infraction.Reason);
                    cmd.Parameters.AddWithValue("?actionId", (int)infraction.Type);
                    cmd.Parameters.AddWithValue("?modId", infraction.ModId);
                    cmd.Parameters.AddWithValue("?guildId", infraction.ServerId);
                    cmd.Parameters.AddWithValue("?userId", infraction.UserId);
                    cmd.Parameters.AddWithValue("?messageId", infraction.MessageId);

                    cmd.ExecuteNonQuery();
                    return Task.CompletedTask;
                }
            }
        }
        

        public static Task SetSocial(ulong uId, string info, int socialid)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    var social = (SPlatform) socialid;
                    switch(social)
                    {
                        case SPlatform.Twitch:
                            cmd.CommandText = "UPDATE `users` SET `twitch` = ?info WHERE (`id` = ?id);";
                            break;
                        case SPlatform.TwitchId:
                            cmd.CommandText = "UPDATE `users` SET `twitch_id` = ?info WHERE (`id` = ?id);";
                            break;
                        case SPlatform.Steam:
                            cmd.CommandText = "UPDATE `users` SET `steam` = ?info WHERE (`id` = ?id);";
                            break;
                        case SPlatform.SteamUrl:
                            cmd.CommandText = "UPDATE `users` SET `steam_url` = ?info WHERE (`id` = ?id);";
                            break;
                        case SPlatform.Twitter:
                            cmd.CommandText = "UPDATE `users` SET `twitter` = ?info WHERE (`id` = ?id);";
                            break;
                        case SPlatform.Switch:
                            cmd.CommandText = "UPDATE `users` SET `switch` = ?info WHERE (`id` = ?id);";
                            break;
                        case SPlatform.PSN:
                            cmd.CommandText = "UPDATE `users` SET `psn` = ?info WHERE (`id` = ?id);";
                            break;
                        case SPlatform.Xbox:
                            cmd.CommandText = "UPDATE `users` SET `xbox` = ?info WHERE (`id` = ?id);";
                            break;
                        default:
                            return Task.CompletedTask;
                    }


                    cmd.Parameters.AddWithValue("?id", uId);
                    cmd.Parameters.AddWithValue("?info", info);

                    cmd.ExecuteNonQuery();
                    return Task.CompletedTask;
                }
            }
        }

        public static Task SetLog(string sId, string info, LogType log)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    switch (log)
                    {
                        case LogType.Infraction:
                            cmd.CommandText = "UPDATE `guild_settings` SET `InfractionLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.Delete:
                            cmd.CommandText = "UPDATE `guild_settings` SET `DeleteLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.Edit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `EditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.ChannelDel:
                            cmd.CommandText = "UPDATE `guild_settings` SET `ChannelDelLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.ChannelAdd:
                            cmd.CommandText = "UPDATE `guild_settings` SET `ChannelAddLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.ChannelEdit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `ChannelEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.RoleDel:
                            cmd.CommandText = "UPDATE `guild_settings` SET `RoleDelLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.RoleAdd:
                            cmd.CommandText = "UPDATE `guild_settings` SET `RoleAddLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.RoleEdit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `RoleEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.UBan:
                            cmd.CommandText = "UPDATE `guild_settings` SET `UBanLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.UUnban:
                            cmd.CommandText = "UPDATE `guild_settings` SET `UUnbanLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.ULeft:
                            cmd.CommandText = "UPDATE `guild_settings` SET `ULeftLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.UJoin:
                            cmd.CommandText = "UPDATE `guild_settings` SET `UJoinLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.MemberEdit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `MemEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.UVoice:
                            cmd.CommandText = "UPDATE `guild_settings` SET `UVoiceLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.GuildEdit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `GuildEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                    }


                    cmd.Parameters.AddWithValue("?id", sId);
                    cmd.Parameters.AddWithValue("?info", info);

                    cmd.ExecuteNonQuery();
                    return Task.CompletedTask;
                }
            }
        }

        public static Task ToggleLog(string sId,bool state, LogType log)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    switch (log)
                    {
                        case LogType.Infraction:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsInfractionLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.Delete:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsDeleteLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.Edit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.ChannelDel:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsChannelDelLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.ChannelAdd:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsChannelAddLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.ChannelEdit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsChannelEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.RoleDel:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsRoleDelLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.RoleAdd:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsRoleAddLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.RoleEdit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsRoleEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.UBan:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsUBanLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.UUnban:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsUUnbanLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.ULeft:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsULeftLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.UJoin:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsUJoinLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.MemberEdit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsMemEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.UVoice:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsUVoiceLog` = ?info WHERE (`ID` = ?id);";
                            break;
                        case LogType.GuildEdit:
                            cmd.CommandText = "UPDATE `guild_settings` SET `IsGuildEditLog` = ?info WHERE (`ID` = ?id);";
                            break;
                    }


                    cmd.Parameters.AddWithValue("?id", sId);
                    cmd.Parameters.AddWithValue("?info", state?1:0);

                    cmd.ExecuteNonQuery();
                    return Task.CompletedTask;
                }
            }
        }
    }
}
