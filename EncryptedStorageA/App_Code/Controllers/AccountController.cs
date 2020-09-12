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
using EncryptedStorage.Data.Models;
using EncryptedStorage.Data.Models.StorageViewModels;
using EncryptedStorageA.App_Code.Abstractions;
using Newtonsoft.Json;

namespace EncryptedStorageA.App_Code.Controllers
{
    public class AccountController : ControllerMemento<List<AccountModel>>
    {
        public List<AccountModel> GetAccounts()
        {
            string url = HttpService.Url;

            try
            {
                var result = HttpService.Instance.GetAsync(url + "Account/GetAccounts");

                if (result.Result.IsSuccessStatusCode)
                {
                    Data = JsonConvert.DeserializeObject<List<AccountModel>>(result.Result.Content.ReadAsStringAsync().Result);
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

        public bool Delete(int id)
        {
            string url = HttpService.Url;

            try
            {
                var result = HttpService.Instance.GetAsync(url + "Account/Delete/"+id);

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
                var result = HttpService.Instance.GetAsync(url + "Account/DeleteAll");

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

        public async Task<bool> Create(string name, string address, string login, string password)
        {

            string url = HttpService.Url;
            try
            {
                var model = new AccountModel()
                {
                    
                    Name = name,
                    Url = address,
                    Login = login,
                    Password = password
                };
                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await HttpService.Instance.PostAsync(url + "Account/Create", content);

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

        public async Task<bool> Change(int id, string name, string address, string login, string password)
        {

            string url = HttpService.Url;
            try
            {
                var model = new AccountModel()
                {
                    Id = id,
                    Name = name,
                    Url = address,
                    Login = login,
                    Password = password
                };
                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await HttpService.Instance.PostAsync(url + "Account/Change", content);

                if (result.IsSuccessStatusCode)
                {
                    return true;
                }

                Exceptions.Add(new Exception(await result.Content.ReadAsStringAsync()));
                return false;
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return false;
            }
        }

        public async Task<string> GetPassword(int id)
        {
            string url = HttpService.Url;

            try
            {
                var result = await HttpService.Instance.GetAsync(url + "Account/GetPassword/" + id);

                if (result.IsSuccessStatusCode)
                {
                    return await result.Content.ReadAsStringAsync();
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
    }
}