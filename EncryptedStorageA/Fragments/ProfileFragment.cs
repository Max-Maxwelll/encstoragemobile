using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EncryptedStorageA.App_Code.Controllers;
using Java.Lang;

namespace EncryptedStorageA.Fragments
{
    public class ProfileFragment : CustomFragment
    {
        private string NAME = "ПРОФИЛЬ";

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup view, Bundle savedInstanceState)
        {
            view = (ViewGroup)inflater.Inflate(Resource.Layout.fragment_profile, null, false);

            ManageController mc = new ManageController();
            var user = mc.GetUser();
            if(user != null)
            {
                view.FindViewById<TextView>(Resource.Id.UserName).Text = user.NormalizedUserName;
                view.FindViewById<TextView>(Resource.Id.UserEmail).Text = user.NormalizedEmail;
                view.FindViewById<TextView>(Resource.Id.Phone).Text = user.PhoneNumber;
                view.FindViewById<Button>(Resource.Id.Save).Click += Save_Click;
            }      

            return view;
        }

        private async void Save_Click(object sender, EventArgs e)
        {
            string email = View.FindViewById<EditText>(Resource.Id.EmailEditText).Text;
            string oldPass = View.FindViewById<EditText>(Resource.Id.OldPasswordEditText).Text;
            string newPass = View.FindViewById<EditText>(Resource.Id.NewPasswordEditText).Text;
            string confirmPass = View.FindViewById<EditText>(Resource.Id.ConfirmPasswordEditText).Text;
            ManageController mc = new ManageController();
            bool result = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                result = await mc.ChangeEmail(email);
                if(!result)
                    Toast.MakeText(Context, mc.Exceptions.FirstOrDefault().Message, ToastLength.Long).Show();
            }
            if(!string.IsNullOrWhiteSpace(oldPass) && !string.IsNullOrWhiteSpace(newPass) && !string.IsNullOrWhiteSpace(confirmPass))
            {
                result = await mc.ChangePassword(oldPass, newPass, confirmPass);
                if (!result)
                    Toast.MakeText(Context, mc.Exceptions.FirstOrDefault().Message, ToastLength.Long).Show();
            }
            if (result)
                Toast.MakeText(Context, "Данные были обновлены", ToastLength.Long).Show();
        }

        public override ICharSequence GetPageTitle()
        {
            return new Java.Lang.String(NAME);
        }
    }
}