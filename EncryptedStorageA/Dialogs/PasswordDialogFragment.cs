using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EncryptedStorageA.App_Code.Controllers;

namespace EncryptedStorageA.Dialogs
{
    class PasswordDialogFragment : Dialog
    {
        public bool Succes { get; private set; }
        public Func<Task<bool>> Func { private get; set; }
        public PasswordDialogFragment(Activity context) : base(context)
        {
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_password);

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
            EditText password = FindViewById<EditText>(Resource.Id.Password);
            UserController userController = new UserController();
            Succes = await userController.ConfirmAction(password.Text);

            if (!Succes)
            {
                Toast.MakeText(Context, "Пароль неверный", ToastLength.Short).Show();
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