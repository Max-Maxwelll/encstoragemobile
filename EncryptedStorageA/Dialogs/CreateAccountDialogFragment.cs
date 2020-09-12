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
using EncryptedStorageA.App_Code.Controllers;

namespace EncryptedStorageA.Dialogs
{
    public class CreateAccountDialogFragment : Dialog
    {
        //public bool Succes { get; private set; }
        public Func<bool> Func { private get; set; }
        public CreateAccountDialogFragment(Activity context) : base(context)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_create_account);

            Button submit = FindViewById<Button>(Resource.Id.Submit);
            Button close = FindViewById<Button>(Resource.Id.Close);
            submit.Click += Submit_Click;
            close.Click += Close_Click;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private async void Submit_Click(object sender, EventArgs e)
        {
            EditText name = FindViewById<EditText>(Resource.Id.Name);
            EditText url = FindViewById<EditText>(Resource.Id.Url);
            EditText login = FindViewById<EditText>(Resource.Id.Login);
            EditText password= FindViewById<EditText>(Resource.Id.Password);
            AccountController ac = new AccountController();
            var result = await ac.Create(name.Text, url.Text, login.Text, password.Text);

            if (!result)
            {
                Toast.MakeText(Context, ac.Exceptions.FirstOrDefault().Message, ToastLength.Short).Show();
                return;
            }
            else
            {
                Cancel();
                Func?.Invoke();
            }
        }
    }
}