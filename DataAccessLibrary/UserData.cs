using DataAccessLibrary.Interfaces;
using DataAccessLibrary.Models;
using SQLite;
using System;
using System.IO;

namespace DataAccessLibrary
{
    public class UserData : DataBase<UserData>
    {
        private static Type[] tables = { typeof(UserModel) };
        public UserData() : base("userdata.db3", tables) { }

        public static UserModel GetUser()
        {
            return Instance.Connection.Table<UserModel>().FirstOrDefault();
        }
    }
}
