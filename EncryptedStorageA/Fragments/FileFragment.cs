using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
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
    public class FileFragment : CustomFragment, IObserver<CustomFragment>
    {
        private string NAME = "ФАЙЛЫ";
        private ListView fileList;
        private int ACTIVITY_CHOOSE_FILE = 22;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup view, Bundle savedInstanceState)
        {
            view = (ViewGroup)inflater.Inflate(Resource.Layout.fragment_files, null, false);

            Button updateFiles = view.FindViewById<Button>(Resource.Id.updateFiles);
            Button createFile = view.FindViewById<Button>(Resource.Id.createFile);
            fileList = view.FindViewById<ListView>(Resource.Id.FileList);

            createFile.Click += CreateFile_Click;
            updateFiles.Click += UpdateFiles_Click;
            UpdateFiles();
            return view;
        }

        private void UpdateFiles_Click(object sender, EventArgs e)
        {
            UpdateFiles();
        }

        public override ICharSequence GetPageTitle()
        {
            return new Java.Lang.String(NAME);
        }

        private void CreateFile_Click(object sender, EventArgs e)
        {
            CreateFileDialogFragment dialog = new CreateFileDialogFragment(Activity);
            dialog.Func = () =>
            {
                Intent i = new Intent(Intent.ActionGetContent);
                i.AddCategory(Intent.CategoryOpenable);
                i.SetType("*/ *");
                
                StartActivityForResult(Intent.CreateChooser(i, "Выбор файла"), ACTIVITY_CHOOSE_FILE);
                return true;
            };
            dialog.Show();
        }

        public async override void OnActivityResult(int requestCode, int resultCode, Intent intent)
        {
            if (resultCode != -1) return;
            if (requestCode == ACTIVITY_CHOOSE_FILE)
            {
                Intent activity = ((Activity)Context).Intent;
                FileController fc = new FileController();
                string type = "";
                if (Path.HasExtension(intent.Data.Path))
                {
                    type = Path.GetExtension(intent.Data.Path);
                }
                bool result = await fc.Upload(intent.Data.Path, activity.GetStringExtra("newName"));

                if (result)
                {
                    UpdateFiles();
                    Toast.MakeText(Context, "Файл загружен", ToastLength.Long).Show();
                }
                else
                {
                    System.Exception ex = fc.Exceptions?.FirstOrDefault();
                    Toast.MakeText(Context, ex.Message + System.Environment.NewLine + ex.StackTrace, ToastLength.Long).Show();
                }
            }
        }

        private async void UpdateFiles()
        {
            FileController fc = new FileController();
            var files = await fc.GetFiles();

            if (files != null)
            {
                FileViewAdapter adapter = new FileViewAdapter(Context, files);
                adapter.FileChange += (e, args) =>
                {
                    UpdateFiles();
                };
                if(fileList != null)
                    fileList.Adapter = adapter;
            }
            else
            {
                FileViewAdapter adapter = new FileViewAdapter(Context, new List<FileModel>());
                fileList.Adapter = adapter;
                adapter.FileChange += (e, args) =>
                {
                    UpdateFiles();
                };
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
            UpdateFiles();
        }
    }
}