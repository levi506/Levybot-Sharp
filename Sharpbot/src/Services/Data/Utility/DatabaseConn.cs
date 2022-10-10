using Npgsql;
using System;

namespace Sharpbot.Services.Data.Utility
{
    public class DatabaseConn : IDisposable
    {

        public static string ConnString { private get; set; }

        private NpgsqlConnection _conn;

        public DatabaseConn()
        {
            _conn = new NpgsqlConnection(ConnString);
            try
            {
                _conn.Open();
            }
            catch (Exception)
            {

            }
        }

        public void Dispose()
        {
            _conn.Dispose();
        }

        public static implicit operator NpgsqlConnection(DatabaseConn conn)
        {
            return conn._conn;
        }


    }


}
