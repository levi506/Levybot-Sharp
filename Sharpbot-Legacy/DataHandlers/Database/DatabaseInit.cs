using System;
using System.Threading.Tasks;
using LevyBotSharp.DiscordHandlers.Plugins.Webhook;
using MySql.Data.MySqlClient;

namespace LevyBotSharp.DataHandlers.Database
{
    public static partial class DataHandler
    {
        public static async Task InitalizeUser(ulong uId)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO `users` (`id`) VALUES (?id);";
                    cmd.Parameters.AddWithValue("?id", uId);

                    Console.WriteLine($"Initalizing {uId}");

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task InitalizeGuild(ulong guildId, ulong botId)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO `guild_settings` (`ID`, `guild`, `bot`) VALUES (?id, ?gid, ?bid);";
                    cmd.Parameters.AddWithValue("?id", DataUtil.ConstructId(guildId, botId));
                    cmd.Parameters.AddWithValue("?gid", guildId);
                    cmd.Parameters.AddWithValue("?bid", botId);

                    Console.WriteLine($"Initalizing {guildId} for {botId}");

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task InitalizeRole(ulong guildId, ulong roleId)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO `guild_roles` (`ID`,`guild`,`role`) VALUES (?Id,?gId,?rId);";
                    cmd.Parameters.AddWithValue("?Id", DataUtil.ConstructId(guildId, roleId));
                    cmd.Parameters.AddWithValue("?gId", guildId);
                    cmd.Parameters.AddWithValue("?rId", roleId);

                    Console.WriteLine($"Initalizing {roleId} for {guildId}");

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        public static async Task InitalizeGuildUser(ulong guildId, ulong userId)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO `guild_members` (`ID`,`guild`,`user`) VALUES (?Id,?gId,?uId);";
                    cmd.Parameters.AddWithValue("?Id", DataUtil.ConstructId(guildId, userId));
                    cmd.Parameters.AddWithValue("?gId", guildId);
                    cmd.Parameters.AddWithValue("?uId", userId);

                    Console.WriteLine($"Initalizing {userId} for {guildId}");

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }


        public static Task InitializeHook()
        {
            return Task.CompletedTask;
        }
    }

    public struct HookCreate
    {
        public HookPurpose purp;
        public string HookToken;
        public ulong HookId;
    }
}
