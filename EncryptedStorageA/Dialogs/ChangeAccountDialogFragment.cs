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
using EncryptedStorage.Data.Models;
using EncryptedStorageA.App_Code.Controllers;

namespace EncryptedStorageA.Dialogs
{
    public class ChangeAccountDialogFragment : Dialog
    {
        //public bool Succes { get; private set; }
        public Func<bool> Func { private get; set; }
        private AccountModel account;
        public ChangeAccountDialogFragment(Activity context, AccountModel account) : base(context)
        {
            this.account = account;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_change_account);

            FindViewById<EditText>(Resource.Id.Name).Text = account.Name;
            FindViewById<EditText>(Resource.Id.Url).Text = account.Url;
            FindViewById<EditText>(Resource.Id.Login).Text = account.Login;
            FindViewById<EditText>(Resource.Id.Password).Text = account.Password;

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

            var result = await ac.Change(account.Id, name.Text, url.Text, login.Text, password.Text);

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