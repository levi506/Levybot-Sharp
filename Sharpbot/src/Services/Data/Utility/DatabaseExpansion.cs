using Discord;
using Npgsql;
using Sharpbot.Services.Logger;
using System.Threading.Tasks;

namespace Sharpbot.Services.Data.Utility
{
    public static class DatabaseExpansion
    {
        public static async Task PostLogAsync(this LogMessage log)
        {
            using NpgsqlConnection conn = new DatabaseConn();

            using NpgsqlCommand command = new NpgsqlCommand
            {
                Connection = conn,
                CommandText = "INSERT INTO `sharpbot`.`logs` (`severity`, `source`, `message`) VALUES(?severity, ?source, ?message);"
            };
            command.Parameters.AddWithValue("?severity", log.Severity);
            command.Parameters.AddWithValue("?source", log.Source);
            command.Parameters.AddWithValue("?message", log.Message);

            try
            {
                command.ExecuteNonQuery();
            } catch (NpgsqlException Error)
            {
                LogManager.ProcessRawLog("Log Expansion", Error.Message, LogSeverity.Critical, Error);
            }
        }
    }
}
