using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using EncryptedStorageA.Adapters;
using Android.Support.V4.App;
using Android.Support.V7.App;
using EncryptedStorageA.Fragments;
using EncryptedStorageA.App_Code.Controllers;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Android;
using Android.Content.PM;

namespace EncryptedStorageA
{
    [Activity(Label = "WorkActivity", Theme = "@style/AppTheme", MainLauncher = false)]
    public class WorkActivity : FragmentActivity
    {
        private UserController user = new UserController();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_work);

            CheckAppPermissions();
            Toolbar toolbar = this.FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);
            ActionBar.Title = "EStorage";

            List<CustomFragment> fragments = new List<CustomFragment>();
            StorageFragment storgeFragment = new StorageFragment();

            AccountFragment accountFragment = new AccountFragment();
            FileFragment fileFragment = new FileFragment();

            storgeFragment.Subscribe(accountFragment);
            storgeFragment.Subscribe(fileFragment);

            fragments.Add(storgeFragment);
            fragments.Add(accountFragment);
            fragments.Add(fileFragment);
            fragments.Add(new ProfileFragment());

            StorageFragmentAdapter storageAdapter = new StorageFragmentAdapter(SupportFragmentManager, fragments);

            ViewPager pager = FindViewById<ViewPager>(Resource.Id.pager);
            pager.Adapter = storageAdapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            this.MenuInflater.Inflate(Resource.Menu.main_menu, menu);

            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.action_exit:
                    Finish();
                    StartActivity(new Intent(this, typeof(MainActivity)));
                    UserData.Instance.Connection.Delete(UserData.GetUser());
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void OnFragmentInteraction()
        {
            throw new NotImplementedException();
        }

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    RequestPermissions(permissions, 1);
                }
            }
        }
    }
}