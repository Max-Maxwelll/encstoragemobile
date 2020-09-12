using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DataAccessLibrary.Models;
using EncryptedStorage.Data.Entities;
using EncryptedStorage.Data.Models;
using EncryptedStorage.Data.Models.StorageViewModels;
using EncryptedStorageA.App_Code.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;

namespace EncryptedStorageA.App_Code.Controllers
{
    class FileController : ControllerMemento<List<FileModel>>
    {
        public async Task<List<FileModel>> GetFiles()
        {
            string url = HttpService.Url;

            try
            {
                var result = await HttpService.Instance.GetAsync(url + "File/GetFiles");

                if (result.IsSuccessStatusCode)
                {
                    Data = JsonConvert.DeserializeObject<List<FileModel>>(await result.Content.ReadAsStringAsync());
                    return Data;
                }

                return null;
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return null;
            }
        }

        public bool Delete(string name)
        {
            string url = HttpService.Url;

            try
            {
                var result = HttpService.Instance.GetAsync(url + "File/Delete/" + name);

                if (result.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Exceptions.Add(new Exception(result.Result.Content.ReadAsStringAsync().Result));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return false;
            }
        }

        public async Task<byte[]> GetEncryptFile(string name, string type)
        {
            string url = HttpService.Url;

            try
            {
                var result = await HttpService.Instance.GetAsync(url + "File/GetEncryptFile/" + name);


                if (result.IsSuccessStatusCode)
                {

                    return await result.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    Exceptions.Add(new Exception(await result.Content.ReadAsStringAsync()));
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return null;
            }
        }

        public async Task<byte[]> GetDecryptFile(string name, string type)
        {
            string url = HttpService.Url;

            try
            {
                var result = await HttpService.Instance.GetAsync(url + "File/GetDecryptFile/" + name);


                if (result.IsSuccessStatusCode)
                {

                    return await result.Content.ReadAsByteArrayAsync();
                }
                else
                {
                    Exceptions.Add(new Exception(await result.Content.ReadAsStringAsync()));
                    return null;
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return null;
            }
        }

        public async Task<bool> Upload(string path, string name)
        {

            string url = HttpService.Url;
            try
            {
                string type = Path.GetExtension(path);


                using (FileStream stream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    var file = new FormFile(stream, 0, stream.Length, name, name + type);
                    using (MultipartFormDataContent content = new MultipartFormDataContent())
                    {
                        content.Add(new StreamContent(stream), name, name + type);
                        //using (BinaryReader br = new BinaryReader(file.OpenReadStream()))
                        // {
                        //ByteArrayContent bytes = new ByteArrayContent(br.ReadBytes((int)file.Length));

                        var result = await HttpService.Instance.PostAsync(url + "File/Upload", content);

                        if (result.IsSuccessStatusCode)
                        {
                            return true;
                        }

                        Exceptions.Add(new Exception(await result.Content.ReadAsStringAsync()));
                        Console.WriteLine(await result.Content.ReadAsStringAsync());
                    }

                    return false;
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return false;
            }
        }
        //public async Task<bool> Upload(string name, string address, string login, string password)
        //{

        //    string url = HttpService.Url;
        //    try
        //    {
        //        var model = new FileModel()
        //        {

        //            Name = name,
        //            Url = address,
        //            Login = login,
        //            Password = password
        //        };
        //        string json = JsonConvert.SerializeObject(model);
        //        HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var result = await HttpService.Instance.PostAsync(url + "Account/Create", content);

        //        if (result.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }

        //        Exceptions.Add(new Exception(result.Content.ReadAsStringAsync().Result));
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        Exceptions.Add(ex);
        //        return false;
        //    }
        //}
    }
}