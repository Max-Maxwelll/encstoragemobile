using Org.Apache.Http.Protocol;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DataAccessLibrary.Interfaces
{
    public abstract class DataBase<T> where T : DataBase<T>, new()
    {
        protected static string name;
        private static Type[] types;
        public static T Instance { get; } = new T();
        private static SQLiteConnection connection;
        public SQLiteConnection Connection
        {
            get
            {
                if (connection == null)
                    connection = new SQLiteConnection(CreateFile(name));
                return connection;
            }
        }


        public DataBase(string name, Type[] types)
        {
            DataBase<T>.name = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), name);
            DataBase<T>.types = types;
        }

        public void CreateTables()
        {
            Connection.CreateTables(CreateFlags.AutoIncPK, types);
        }

        private static string CreateFile(string url)
        {
            Console.WriteLine("###URL" + url);
            if (!File.Exists(url))
                File.Create(url);

            return url;
        }
    }
}
