using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using EncryptedStorage.Data.Models;
using EncryptedStorageA.App_Code.Controllers;
using EncryptedStorageA.Dialogs;

namespace EncryptedStorageA.Adapters
{
    public class AccountsViewAdapter : BaseAdapter<AccountModel>
    {
        private Context context;
        private List<AccountModel> accounts;
        public event EventHandler AccountChange;

        public AccountsViewAdapter(Context context, List<AccountModel> accounts)
        {
            this.context = context;
            this.accounts = accounts;
        }

        public override long GetItemId(int position) { return position; }

        public override View GetView(int position, View view, ViewGroup parent)
        {
            var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();

            view = inflater.Inflate(Resource.Layout.account_item, null, false);
            view.LongClick += (sender, e) =>
            {
                PopupMenu menu = new PopupMenu(context, view, GravityFlags.Right);
                menu.Inflate(Resource.Menu.account_menu);
                menu.MenuItemClick += delegate(object s, PopupMenu.MenuItemClickEventArgs args) { Menu_MenuItemClick(s, args, position); };
                menu.Show();
            };

            TextView name = view.FindViewById<TextView>(Resource.Id.NameAccount);
            TextView url = view.FindViewById<TextView>(Resource.Id.Url);
            TextView Login = view.FindViewById<TextView>(Resource.Id.Login);

            name.Text = this[position].Name;
            url.Text = this[position].Url;
            Login.Text = this[position].Login;

            return view;
        }

        private async void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e, int position)
        {
            if (e.Item.ItemId == Resource.Id.action_delete)
            {
                PasswordDialogFragment dialog = new PasswordDialogFragment((Activity)context);
                dialog.Func = () =>
                {
                    AccountController storageController = new AccountController();
                    bool result = storageController.Delete(this[position].Id);
                    if (result)
                    {
                        AccountChange(this, new EventArgs());
                        Toast.MakeText(context, "Аккаунт " + this[position].Name + " удален", ToastLength.Long).Show();
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
                ChangeAccountDialogFragment dialog = new ChangeAccountDialogFragment((Activity)context, this[position]);
                dialog.Func = () =>
                {
                    AccountChange(this, new EventArgs());
                    Toast.MakeText(context, "Аккаунт успешно изменен", ToastLength.Long).Show();
                    return true;
                };
                dialog.Show();
            }
            else if (e.Item.ItemId == Resource.Id.action_get_password)
            {
                ClipboardManager clipboard = (ClipboardManager)context.GetSystemService(Context.ClipboardService);
                AccountController ac = new AccountController();
                string result = await ac.GetPassword(this[position].Id);
                if (!string.IsNullOrWhiteSpace(result))
                {
                    ClipData clip = ClipData.NewPlainText("password", result);
                    clipboard.PrimaryClip = clip;
                    Toast.MakeText(context, "Пароль скопирован", ToastLength.Long).Show();
                }
            }
        }

        public override AccountModel this[int position] { get { return accounts[position]; } }

        public override int Count { get{ return accounts.Count; } }
    }
}