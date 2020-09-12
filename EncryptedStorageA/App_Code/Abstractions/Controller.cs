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
    public abstract class Controller
    {
        public List<Exception> Exceptions { get; private set; } = new List<Exception>();
    }
}