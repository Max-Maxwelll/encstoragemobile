using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using EncryptedStorageA.App_Code.Controllers;
using System.Threading.Tasks;
using System.Threading;
using DataAccessLibrary.Interfaces;
using System.Net;
using System;
using Newtonsoft.Json;
using Android;
using Android.Content.PM;

namespace EncryptedStorageA
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public static int REQUEST_CODE_SIGNIN = 10;
        public static int REQUEST_CODE_WORK = 20;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            ServicePointManager.DefaultConnectionLimit = 5;
            UserData.Instance.CreateTables();
            SessionController sc = new SessionController();

            UserModel model = UserData.Instance.Connection.Table<UserModel>().FirstOrDefault();

            if (model == null || string.IsNullOrWhiteSpace(model.Session))
            {
                Intent intent = new Intent(this, typeof(SigninActivity));
                StartActivityForResult(intent, REQUEST_CODE_SIGNIN);
            }
            else
            {
                ManageController mc = new ManageController();

                bool result = await sc.RestoreSession(model.Session);
                if (result)
                {
                    var user = await mc.GetUserAsync();

                    if (user != null)
                    {
                        mc.SetSession(model.Session);
                        mc.Save<UserData>();
                        Intent intent = new Intent(this, typeof(WorkActivity));
                        StartActivityForResult(intent, REQUEST_CODE_WORK);
                    }
                    else
                    {
                        Intent intent = new Intent(this, typeof(SigninActivity));
                        StartActivityForResult(intent, REQUEST_CODE_SIGNIN);
                    }
                }
                else Toast.MakeText(this, "Что-то пошло нетак", ToastLength.Long).Show();
            }
        }

        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == REQUEST_CODE_SIGNIN)
            {
                if (resultCode == Result.Ok)
                {
                    Intent intent = new Intent(this, typeof(WorkActivity));
                    StartActivityForResult(intent, REQUEST_CODE_WORK);
                }
                else
                {
                    Toast.MakeText(this, "Авторизация не произведена", ToastLength.Long).Show();
                    Intent intent = new Intent(this, typeof(SigninActivity));
                    StartActivityForResult(intent, REQUEST_CODE_SIGNIN);
                }
            }
        }
    }
}