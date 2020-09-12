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
using EncryptedStorage.Data.Models.StorageViewModels;
using EncryptedStorageA.App_Code.Controllers;

namespace EncryptedStorageA.Dialogs
{
    class ChangeKeyDialogFragment : Dialog
    {
        public bool Succes { get; private set; }
        public Func<bool> Func { private get; set; }
        private string storage;
        public ChangeKeyDialogFragment(Activity context, string storage) : base(context)
        {
            this.storage = storage;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.dialog_change_key);

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
            EditText oldKey = FindViewById<EditText>(Resource.Id.OldKey);
            EditText newKey = FindViewById<EditText>(Resource.Id.NewKey);
            EditText confirmKey = FindViewById<EditText>(Resource.Id.ConfirmKey);
            StorageController sc = new StorageController();
            
            Succes = await sc.ChangeKey(storage, oldKey.Text, newKey.Text, confirmKey.Text);

            if (!Succes)
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