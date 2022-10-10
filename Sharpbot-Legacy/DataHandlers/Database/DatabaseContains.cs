using MySql.Data.MySqlClient;
using System.Threading.Tasks;

namespace LevyBotSharp.DataHandlers.Database
{
    public static partial class DataHandler
    {

        public static async Task<bool> ContainsTwitch(string twitchId)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.CommandText = "SELECT * IN `users` WHERE `twitch_id` = ?id;";

                    cmd.Parameters.AddWithValue("?id", twitchId);

                    var result = await cmd.ExecuteScalarAsync();
                    return result != null;
                }
            }
        }

        public static async Task<bool> IsBanned(ulong uId)
        {
            using (var conn = new DatabaseConn())
            {
                using (var cmd = new MySqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandText = $"SELECT perms FROM `users` WHERE `id`=?id;";
                    cmd.Parameters.AddWithValue("?id", uId);

                    var check = cmd.ExecuteScalar();
                    if (check != null)
                    {
                        var reader = cmd.ExecuteReader();

                        while (await reader.ReadAsync())
                            return reader.GetInt32("perms") == 7;
                    }

                    await InitalizeUser(uId);

                    return false;
                }
            }

        }
    }
}
