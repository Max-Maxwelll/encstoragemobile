using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models;
using EncryptedStorage.Data.Models.AccountViewModels;
using EncryptedStorageA.App_Code.Abstractions;
using Newtonsoft.Json;

namespace EncryptedStorageA.App_Code.Controllers
{
    public class UserController : Controller
    {
        public async Task<bool> SignIn(string login, string password)
        {
            string url = HttpService.Url;
            LoginModel model = new LoginModel()
            {
                Username = login,
                Password = password
            };
            string json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var result = await HttpService.Instance.PostAsync(url + "User/Login", content);

                if (result.IsSuccessStatusCode)
                    return true;
                else
                {
                    Exceptions.Add(new Exception(result.Content.ReadAsStringAsync().Result));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return false;
            }
        }

        public bool Logout()
        {
            string url = HttpService.Url;
            var result = HttpService.Instance.GetAsync(url + "User/Logout");

            if (result.Result.IsSuccessStatusCode)
            {
                HttpService.Clear();
                return true;
            }

            return false;
        }

        public async Task<bool> ConfirmAction(string password)
        {
            string url = HttpService.Url;
            ConfirmActionViewModels model = new ConfirmActionViewModels()
            {
                Password = password
            };

            string json = JsonConvert.SerializeObject(model);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = await HttpService.Instance.PostAsync(url + "User/ConfirmAction", content);

            if (result.IsSuccessStatusCode)
                return true;
            else
            {
                Exceptions.Add(new Exception(result.Content.ReadAsStringAsync().Result));
                return false;
            }
        }
    }
}