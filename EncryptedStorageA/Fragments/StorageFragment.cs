using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using EncryptedStorage.Data.Entities;
using EncryptedStorageA.Adapters;
using EncryptedStorageA.App_Code.Controllers;
using EncryptedStorageA.Dialogs;
using Java.Lang;

namespace EncryptedStorageA.Fragments
{
    public class StorageFragment : CustomFragment, IObservable<CustomFragment>
    {
        private string NAME = "ХРАНИЛИЩА";
        private List<IObserver<CustomFragment>> observers = new List<IObserver<CustomFragment>>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup view, Bundle savedInstanceState)
        {
            view = (ViewGroup)inflater.Inflate(Resource.Layout.fragment_storages, null, false);

            Button updateStorages = view.FindViewById<Button>(Resource.Id.updateStorages);
            Button createStorage = view.FindViewById<Button>(Resource.Id.createStorage);
            Button deleteAllStorages = view.FindViewById<Button>(Resource.Id.deleteAllStorages);

            updateStorages.Click += delegate(object sender, EventArgs e) { UpdateStorages_Click(sender, e, view); };
            createStorage.Click += delegate (object sender, EventArgs e) { CreateStorage_Click(sender, e, view); };
            deleteAllStorages.Click += delegate (object sender, EventArgs e) { DeleteAllStorages_Click(sender, e, view); };

            UpdateStorages(view);

            return view;
        }

        private void DeleteAllStorages_Click(object sender, EventArgs e, ViewGroup view)
        {
            PasswordDialogFragment dialog = new PasswordDialogFragment(Activity);
            dialog.Func = () =>
            {
                StorageController storageController = new StorageController();
                bool result = storageController.DeleteAll();
                if (result)
                {
                    UpdateStorages(view);
                    Toast.MakeText(Context, "Все хранилища удалены", ToastLength.Long).Show();
                    return new Task<bool>(()=> true);
                }
                else
                {
                    Toast.MakeText(Context, "Неудача", ToastLength.Long).Show();
                    return new Task<bool>(() => false);
                }
            };
            dialog.Show();
        }

        private void CreateStorage_Click(object sender, EventArgs e, ViewGroup view)
        {
            CreateStorageDialogFragment dialog = new CreateStorageDialogFragment(Activity);
            dialog.Func = () =>
            {
                UpdateStorages(view);
                Toast.MakeText(Context, "Хранилище создано", ToastLength.Long).Show();
                return true;
            };
            dialog.Show();
        }

        public override ICharSequence GetPageTitle()
        {
            return new Java.Lang.String(NAME);
        }

        private void UpdateStorages_Click(object sender, EventArgs e, ViewGroup container)
        {
            UpdateStorages(container);
        }

        private void UpdateStorages(ViewGroup container)
        {
            ListView storageList = container.FindViewById<ListView>(Resource.Id.StorageList);
            StorageController sc = new StorageController();
            var storages = sc.GetStorages();

            if (storages != null)
            {
                StoragesViewAdapter adapter = new StoragesViewAdapter(Context, storages);
                adapter.StorageChange += (e, args) => UpdateStorages(container);
                adapter.EnterKey += (e, args) =>
                {
                    foreach (var observer in observers.ToArray())
                        if (observers.Contains(observer))
                            observer.OnCompleted();
                };
                if (storageList != null)
                    storageList.Adapter = adapter;
            }
            else
            {
                StoragesViewAdapter adapter = new StoragesViewAdapter(Context, new List<StorageModel>());
                storageList.Adapter = adapter;
                adapter.StorageChange += (e, args) => UpdateStorages(container);
                adapter.EnterKey += (e, args) =>
                {
                    foreach (var observer in observers.ToArray())
                        if (observers.Contains(observer))
                            observer.OnCompleted();
                };
                Toast.MakeText(Context, sc.Exceptions.FirstOrDefault().Message, ToastLength.Long).Show();
            }
        }

        public IDisposable Subscribe(IObserver<CustomFragment> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }
    }
}