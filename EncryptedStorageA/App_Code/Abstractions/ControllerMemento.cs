using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DataAccessLibrary.Interfaces;

namespace EncryptedStorageA.App_Code.Abstractions
{
    public abstract class ControllerMemento<T> : Controller
    {
        public T Data { get; protected set; }

        public void Save<DB>() where DB : DataBase<DB>, new()
        {
            DataBase<DB>.Instance.Connection.InsertOrReplace(Data);
        }
    }
}