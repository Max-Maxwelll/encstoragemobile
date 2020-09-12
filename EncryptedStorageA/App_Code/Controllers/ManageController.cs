using DataAccessLibrary.Models;
using EncryptedStorage.Data.Models.ManageViewModels;
using EncryptedStorageA.App_Code.Abstractions;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EncryptedStorageA.App_Code.Controllers
{
    public class ManageController : ControllerMemento<UserModel>
    {
        public async Task<UserModel> GetUserAsync()
        {
            var url = HttpService.Url;
            var result = await HttpService.Instance.GetAsync(url + "Manage/GetUser");

            if (result.IsSuccessStatusCode)
            {
                string content = await result.Content.ReadAsStringAsync();

                Data = JsonConvert.DeserializeObject<UserModel>(content);
                if (Data == null)
                    return null;

                return Data;
            }

            return null;
        }

        public UserModel GetUser()
        {
            var url = HttpService.Url;
            var result = HttpService.Instance.GetAsync(url + "Manage/GetUser");

            if (result.Result.IsSuccessStatusCode)
            {
                string content = result.Result.Content.ReadAsStringAsync().Result;

                Data = JsonConvert.DeserializeObject<UserModel>(content);
                if (Data == null)
                    return null;

                return Data;
            }

            return null;
        }

        public async Task<bool> ChangeEmail(string email)
        {
            string url = HttpService.Url;
            try
            {
                var model = new EmailViewModel()
                {
                    Username = "",
                    Email = email,
                    IsEmailConfirmed = true,
                    StatusMessage = ""
                };

                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await HttpService.Instance.PostAsync(url + "Manage/ChangeEmail", content);

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

        public async Task<bool> ChangePassword(string oldPass, string newPass, string confirmPass)
        {
            string url = HttpService.Url;
            try
            {
                var model = new ChangePasswordViewModel()
                {
                    OldPassword = oldPass,
                    NewPassword = newPass,
                    ConfirmPassword = confirmPass
                };

                string json = JsonConvert.SerializeObject(model);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await HttpService.Instance.PostAsync(url + "Manage/ChangePassword", content);

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


        public void SetSession(string session)
        {
            if(!string.IsNullOrWhiteSpace(session))
                Data.Session = session;
        }
    }
}