using Android.Support.V7.App;
using Android.App;
using Android.OS;
using Android.Widget;
using EncryptedStorageA.App_Code;
using System;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using EncryptedStorageA.App_Code.Controllers;
using Newtonsoft.Json;

namespace EncryptedStorageA
{
    [Activity(Label = "SigninActivity", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SigninActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_signin);

            Button signinButton = FindViewById<Button>(Resource.Id.signin);

            signinButton.Click += SigninButton_Click;

            UserData.Instance.Connection.CreateTables();
            Console.WriteLine(UserData.Instance.Connection.Table<UserModel>().Count());
        }

        private async void SigninButton_Click(object sender, System.EventArgs e)
        {
            EditText userId = FindViewById<EditText>(Resource.Id.userId);
            EditText password = FindViewById<EditText>(Resource.Id.userPassword);
            SessionController sc = new SessionController();
            UserController uc = new UserController();
            ManageController mc = new ManageController();

            bool result = await uc.SignIn(userId.Text, password.Text);
            
            if (result)
            {
                var user = await mc.GetUserAsync();

                if (user != null)
                {
                    string session = await sc.CreateSession();

                    if (string.IsNullOrWhiteSpace(session))
                    { 
                        return;
                    }
                    mc.SetSession(session);
                    
                    mc.Save<UserData>();
                }
                SetResult(Result.Ok);
                Finish();
            }
            else
            {
                Toast.MakeText(this, "Не удалось авторизироватся", ToastLength.Long).Show();
            }
                
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SetResult(Result.Canceled);
        }
    }
}