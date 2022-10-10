using System;
using MySql.Data.MySqlClient;

namespace LevyBotSharp.DataHandlers.Database
{
    public class DatabaseConn : IDisposable
    {

        public static string ConnString { private get; set; }

        private MySqlConnection _conn;

        public DatabaseConn()
        {
            _conn = new MySqlConnection(ConnString);
            try
            {
                _conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void Dispose()
        {
            _conn.Dispose();
        }

        public static implicit operator MySqlConnection(DatabaseConn conn)
        {
            return conn._conn;
        }


    }


}
