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
    public class CreateFileDialogFragment : Dialog
    {
        private Activity context;
        public Func<bool> Func;
        private int ACTIVITY_CHOOSE_FILE = 22;

        public CreateFileDialogFragment(Activity context) : base(context)
        {
            this.context = context;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_create_file);

            Button submit = FindViewById<Button>(Resource.Id.Submit);
            Button close = FindViewById<Button>(Resource.Id.Close);
            submit.Click += Submit_Click;
            close.Click += Close_Click;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            Cancel();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            EditText name = FindViewById<EditText>(Resource.Id.Name);
            context.Intent.PutExtra("newName", name.Text);
            if (string.IsNullOrWhiteSpace(name.Text))
            {
                Toast.MakeText(context, "Имя файла не может быть пустым", ToastLength.Long).Show();
                return;
            }
                
            Func?.Invoke();
            Cancel();
        }
    }
}