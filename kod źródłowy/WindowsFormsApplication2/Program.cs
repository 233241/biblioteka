using System;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace Biblioteka
{
    static class Program
    {
        static string server = "127.0.0.1";
        static string port = "5432";
        static string database = "postgres";
        


        public static DataBase db;
        public static User currentUser;
        public static Form_okno_glowne oknoGlowne;

        public static void connect()
        {
            db = new DataBase(server, port, currentUser.rola, currentUser.rola, database);
            db.connect();
        }

        public static void disconnect()
        {
            db.close();
        }

        public static string md5(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
                strBuilder.Append(result[i].ToString("x2"));

            return strBuilder.ToString();
        }

        static void Main()
        {
            var lines = File.ReadAllLines("Database.conf");

            server = lines[0];
            port = lines[1];
            database = lines[2];

            currentUser = new User();
            connect();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            oknoGlowne = new Form_okno_glowne();
            Application.Run(oknoGlowne);

            disconnect();
        }
    }
}
