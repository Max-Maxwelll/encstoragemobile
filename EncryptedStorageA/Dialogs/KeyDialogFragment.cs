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
    class KeyDialogFragment : Dialog
    {
        public bool Succes { get; private set; }
        public string storage;
        public Func<bool> Func { private get; set; }
        public KeyDialogFragment(Activity context, string storage) : base(context)
        {
            this.storage = storage;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_key);

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
            EditText key = FindViewById<EditText>(Resource.Id.Key);

            StorageController sc = new StorageController();
            bool result = sc.EnterKey(storage, key.Text);

            if (!result)
            {
                Toast.MakeText(Context, sc.Exceptions.FirstOrDefault().Message, ToastLength.Short).Show();
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