using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using EncryptedStorageA.Fragments;

namespace EncryptedStorageA.Adapters
{
    class StorageFragmentAdapter : FragmentPagerAdapter
    {
        private List<CustomFragment> fragments;
        public StorageFragmentAdapter(Android.Support.V4.App.FragmentManager manager, List<CustomFragment> fragments)
            :base(manager)
        {
            this.fragments = fragments;
        }

        public override int Count { get { return fragments.Count; } }

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {
            return fragments[position];
        }

        public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
        {
            return fragments[position].GetPageTitle();
        }
    }
}