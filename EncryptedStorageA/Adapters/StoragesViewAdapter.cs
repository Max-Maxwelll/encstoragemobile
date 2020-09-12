using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using EncryptedStorage.Data.Entities;
using EncryptedStorageA.App_Code;
using EncryptedStorageA.App_Code.Controllers;
using EncryptedStorageA.Dialogs;
using EncryptedStorageA.Fragments;

namespace EncryptedStorageA.Adapters
{
    public class StoragesViewAdapter : BaseAdapter<StorageModel>
    {
        private Context context;
        private List<StorageModel> storages;
        public event EventHandler StorageChange;
        public event EventHandler EnterKey;

        public StoragesViewAdapter(Context context, List<StorageModel> storages)
        {
            this.context = context;
            this.storages = storages;
        }

        public override long GetItemId(int position) { return position; }

        public override View GetView(int position, View view, ViewGroup parent)
        {
            var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

            view = inflater.Inflate(Resource.Layout.storage_item, null, false);
            view.LongClick += (sender, e) =>
            {
                PopupMenu menu = new PopupMenu(context, view, GravityFlags.Right);
                menu.Inflate(Resource.Menu.storage_menu);
                menu.MenuItemClick += delegate (object s, PopupMenu.MenuItemClickEventArgs args) { Menu_MenuItemClick(s, args, position); };
                menu.Show();
                
                //int colorRes = ContextCompat.GetColor(context, Resource.Color.colorPrimary);
                //Android.Graphics.Color color = new Android.Graphics.Color(colorRes);
                //view.FindViewById<TextView>(Resource.Id.NameStorage).SetTextColor(color);
            };
            TextView name = view.FindViewById<TextView>(Resource.Id.NameStorage);
            name.Text = storages[position].Name;

            return view;
        }

        private void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e, int position)
        {
            if (e.Item.ItemId == Resource.Id.action_delete)
            {
                PasswordDialogFragment dialog = new PasswordDialogFragment((Activity)context);
                dialog.Func = () =>
                {
                    StorageController storageController = new StorageController();
                    bool result = storageController.Delete(storages[position].Name);
                    if (result)
                    {
                        StorageChange(this, new EventArgs());
                        Toast.MakeText(context, "Хранилище " + storages[position].Name + " удалено", ToastLength.Long).Show();
                        return new Task<bool>(() => true);
                    }
                    else
                    {
                        Toast.MakeText(context, "Неудача", ToastLength.Long).Show();
                        return new Task<bool>(() => false);
                    }
                };
                dialog.Show();
            }
            else if (e.Item.ItemId == Resource.Id.action_change)
            {
                ChangeKeyDialogFragment dialog = new ChangeKeyDialogFragment((Activity)context, storages[position].Name);
                dialog.Func = () =>
                {
                    Toast.MakeText(context, "Ключ успешно изменен", ToastLength.Long).Show();
                    return true;
                };
                dialog.Show();
            }
            else if (e.Item.ItemId == Resource.Id.action_connect)
            {
                KeyDialogFragment dialog = new KeyDialogFragment((Activity)context, storages[position].Name);
                dialog.Func = () =>
                {
                    EnterKey(this, new EventArgs());
                    Toast.MakeText(context, "Вы вошли в хранилище", ToastLength.Long).Show();
                    return true;
                };
                dialog.Show();
            }
        }

        public override StorageModel this[int position] { get { return storages[position]; } }

        public override int Count { get{ return storages.Count; } }
    }

    internal class StoragesViewAdapterViewHolder : Java.Lang.Object
    {
    }
}