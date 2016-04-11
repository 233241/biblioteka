using Npgsql;
using System;

namespace Biblioteka
{
    class DataBase
    {
        private string connectData;
        private NpgsqlConnection conn;
        private NpgsqlCommand command;

        public DataBase(string server, string port, string user, string password, string name)
        {
            connectData =   "Server="+ server + 
                            ";Port=" + port + 
                            ";User Id=" + user + 
                            ";Password=" + password + 
                            ";Database=" + name + ";";
        }

        public void connect()
        {
            conn = new NpgsqlConnection(connectData);
            conn.Open();
        }

        public Int32 executeNonQuery(string query)
        {
            command = new NpgsqlCommand(query, conn);
            return command.ExecuteNonQuery();
        }

        public NpgsqlDataReader execute(string query)
        {
            command = new NpgsqlCommand(query, conn);
            return command.ExecuteReader();
        }

        public void close()
        {
            conn.Close();
        }
    }
}
