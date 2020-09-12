using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EncryptedStorage.Data.Models;
using EncryptedStorageA.App_Code.Controllers;
using EncryptedStorageA.Dialogs;

namespace EncryptedStorageA.Adapters
{
    public class FileViewAdapter : BaseAdapter<FileModel>
    {
        private Context context;
        private List<FileModel> files;
        public event EventHandler FileChange;

        public FileViewAdapter(Context context, List<FileModel> files)
        {
            this.context = context;
            this.files = files;
        }

        public override long GetItemId(int position) { return position; }

        public override View GetView(int position, View view, ViewGroup parent)
        {
            var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

            view = inflater.Inflate(Resource.Layout.file_item, null, false);
            view.LongClick += (sender, e) =>
            {
                PopupMenu menu = new PopupMenu(context, view, GravityFlags.Right);
                menu.Inflate(Resource.Menu.file_menu);
                menu.MenuItemClick += delegate(object s, PopupMenu.MenuItemClickEventArgs args) { Menu_MenuItemClick(s, args, position); };
                menu.Show();
            };

            TextView icon = view.FindViewById<TextView>(Resource.Id.Icon);
            TextView name = view.FindViewById<TextView>(Resource.Id.Name);
            TextView size = view.FindViewById<TextView>(Resource.Id.Size);

            icon.Text = this[position].Type;
            name.Text = this[position].Name;
            size.Text = this[position].Size.ToString() + " Mb";

            return view;
        }

        private void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e, int position)
        {
            if (e.Item.ItemId == Resource.Id.action_delete)
            {
                PasswordDialogFragment dialog = new PasswordDialogFragment((Activity)context);
                dialog.Func = () =>
                {
                    FileController fc = new FileController();
                    bool result = fc.Delete(this[position].Name);
                    if (result)
                    {
                        FileChange(this, new EventArgs());
                        Toast.MakeText(context, "Файл " + this[position].Name + " удален", ToastLength.Long).Show();
                        return new Task<bool>(() => true);
                    }
                    else
                    {
                        Toast.MakeText(context, fc.Exceptions.FirstOrDefault().Message, ToastLength.Long).Show();
                        return new Task<bool>(() => false);
                    }
                };
                dialog.Show();
            }
            else if (e.Item.ItemId == Resource.Id.action_get_decrypt)
            {
                PasswordDialogFragment dialog = new PasswordDialogFragment((Activity)context);
                dialog.Func = async () =>
                {
                    FileController fc = new FileController();
                    byte[] result = await fc.GetDecryptFile(this[position].Name, this[position].Type);

                    if (result != null)
                    {
                        bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);
                        if (!isWriteable)
                            return false;
                        string path = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, this[position].Name + "." + this[position].Type);

                        using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            await stream.WriteAsync(result, 0, result.Length);
                        }
                        Toast.MakeText(context, "Файл " + this[position].Name + " загружен", ToastLength.Long).Show();
                        return true;
                    }
                    else
                    {
                        Toast.MakeText(context, fc.Exceptions.FirstOrDefault().Message, ToastLength.Long).Show();
                        return false;
                    }
                };
                dialog.Show();
            }
            else if (e.Item.ItemId == Resource.Id.action_get_encrypt)
            {
                PasswordDialogFragment dialog = new PasswordDialogFragment((Activity)context);
                dialog.Func = async () =>
                {
                    FileController fc = new FileController();
                    byte[] result = await fc.GetEncryptFile(this[position].Name, this[position].Type);
                    
                    if (result != null)
                    {
                        bool isWriteable = Android.OS.Environment.MediaMounted.Equals(Android.OS.Environment.ExternalStorageState);
                        if (!isWriteable)
                            return false;
                        string path = Path.Combine(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath, this[position].Name + "." + this[position].Type);

                        using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                        {
                            stream.WriteAsync(result, 0, result.Length);
                        }
                        Toast.MakeText(context, "Файл " + this[position].Name + " загружен", ToastLength.Long).Show();
                        return true;
                    }
                    else
                    {
                        Toast.MakeText(context, fc.Exceptions.FirstOrDefault().Message, ToastLength.Long).Show();
                        return false;
                    }
                };
                dialog.Show();
            }
        }

        public override FileModel this[int position] { get { return files[position]; } }

        public override int Count { get{ return files.Count; } }
    }
}