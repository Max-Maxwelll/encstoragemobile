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
using EncryptedStorage.Data.Models;
using EncryptedStorageA.Adapters;
using EncryptedStorageA.App_Code.Controllers;
using EncryptedStorageA.Dialogs;
using Java.Lang;

namespace EncryptedStorageA.Fragments
{
    public class AccountFragment : CustomFragment, IObserver<CustomFragment>
    {
        private string NAME = "АККАУНТЫ";
        private ListView accountList;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup view, Bundle savedInstanceState)
        {
            view = (ViewGroup)inflater.Inflate(Resource.Layout.fragment_accounts, null, false);

            Button updateAccounts = view.FindViewById<Button>(Resource.Id.updateAccounts);
            Button createAccount = view.FindViewById<Button>(Resource.Id.createAccount);
            Button deleteAllAccounts = view.FindViewById<Button>(Resource.Id.deleteAllAccounts);
            accountList = view.FindViewById<ListView>(Resource.Id.AccountList);

            createAccount.Click += CreateAccount_Click;
            deleteAllAccounts.Click += DeleteAllAccounts_Click;
            updateAccounts.Click += UpdateAccounts_Click;
            UpdateAccounts();
            return view;
        }

        private void DeleteAllAccounts_Click(object sender, EventArgs e)
        {
            PasswordDialogFragment dialog = new PasswordDialogFragment(Activity);
            dialog.Func = () =>
            {
                AccountController storageController = new AccountController();
                bool result = storageController.DeleteAll();
                if (result)
                {
                    UpdateAccounts();
                    Toast.MakeText(Context, "Все аккаунты удалены", ToastLength.Long).Show();
                    return new Task<bool>(() => true);
                }
                else
                {
                    Toast.MakeText(Context, "Неудача", ToastLength.Long).Show();
                    return new Task<bool>(() => false);
                }
            };
            dialog.Show();
        }

        private void CreateAccount_Click(object sender, EventArgs e)
        {
            CreateAccountDialogFragment dialog = new CreateAccountDialogFragment(Activity);
            dialog.Func = () =>
            {
                UpdateAccounts();
                Toast.MakeText(Context, "Аккаунт создан", ToastLength.Long).Show();
                return true;
            };
            dialog.Show();
        }

        private void UpdateAccounts_Click(object sender, EventArgs e)
        {
            UpdateAccounts();
        }

        public override ICharSequence GetPageTitle()
        {
            return new Java.Lang.String(NAME);
        }

        private void UpdateAccounts()
        {
            AccountController ac = new AccountController();
            var accounts = ac.GetAccounts();

            if (accounts != null)
            {
                AccountsViewAdapter adapter = new AccountsViewAdapter(Context, accounts);
                adapter.AccountChange += (e, args) =>
                {
                    UpdateAccounts();
                };
                if (accountList != null)
                    accountList.Adapter = adapter;
            }
            else
            {
                AccountsViewAdapter adapter = new AccountsViewAdapter(Context, new List<AccountModel>());
                accountList.Adapter = adapter;
                adapter.AccountChange += (e, args) =>
                {
                    UpdateAccounts();
                };
                //Toast.MakeText(Context, ac.Exceptions.FirstOrDefault().Message, ToastLength.Long).Show();
            }
        }

        public void OnNext(CustomFragment value)
        {
            Console.WriteLine(value.GetPageTitle());
        }

        public void OnError(System.Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            UpdateAccounts();
        }
    }
}