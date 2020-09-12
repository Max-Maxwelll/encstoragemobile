using System;
using System.Collections.Generic;
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
using EncryptedStorage.Data.Models.StorageViewModels;
using EncryptedStorageA.App_Code.Abstractions;
using Newtonsoft.Json;

namespace EncryptedStorageA.App_Code.Controllers
{
    public class StorageController : ControllerMemento<List<StorageModel>>
    {
        public List<StorageModel> GetStorages()
        {
            string url = HttpService.Url;

            try
            {
                var result = HttpService.Instance.GetAsync(url + "Storage/GetStorages");
                var user = HttpService.Instance.GetAsync(url + "Manage/GetUser");

                if (result.Result.IsSuccessStatusCode)
                {
                    Data = JsonConvert.DeserializeObject<List<StorageModel>>(result.Result.Content.ReadAsStringAsync().Result);
                    return Data;
                }
                else
                {
                    Exceptions.Add(new Exception(user.Result.Content.ReadAsStringAsync().Result));
                    return null;
                }
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
                var result = HttpService.Instance.GetAsync(url + "Storage/Delete/" + name);

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

        public bool DeleteAll()
        {
            string url = HttpService.Url;

            try
            {
                var result = HttpService.Instance.GetAsync(url + "Storage/DeleteAll");

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

        public async Task<bool> Create(string user, string name, string key)
        {
            
            string url = HttpService.Url;
            try
            {
                var model = new StorageRequestModel()
                {
                    Storage = new StorageModel()
                    {
                        User = user,
                        Name = name
                    },
                    Key = key
                };
                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await HttpService.Instance.PostAsync(url + "Storage/CreateStorage", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                Exceptions.Add(new Exception(result.Content.ReadAsStringAsync().Result));
                return false;
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return false;
            }
        }

        public async Task<bool> ChangeKey(string storage, string oldKey, string newKey, string confirmKey)
        {
            string url = HttpService.Url;
            try
            {
                var model = new ChangeKeyStorageViewModel()
                {
                    Name = storage,
                    OldKey = oldKey, 
                    NewKey = newKey,
                    ConfirmKey = confirmKey
                };
                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await HttpService.Instance.PostAsync(url + "Storage/ChangeKeyStorage", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                Exceptions.Add(new Exception(result.Content.ReadAsStringAsync().Result));
                return false;
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return false;
            }
        }

        public bool EnterKey(string storage, string key)
        {
            string url = HttpService.Url;
            try
            {
                var model = new KeyViewModel()
                {
                    StorageName = storage,
                    Key = key
                };
                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = HttpService.Instance.PostAsync(url + "Storage/EnterKey", content).Result;

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                Exceptions.Add(new Exception(result.Content.ReadAsStringAsync().Result));
                return false;
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return false;
            }
        }
    }
}