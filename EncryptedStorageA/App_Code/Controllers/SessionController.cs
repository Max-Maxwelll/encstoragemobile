using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models;
using EncryptedStorageA.App_Code.Abstractions;
using Newtonsoft.Json;

namespace EncryptedStorageA.App_Code.Controllers
{
    public class SessionController : Controller
    {
        public async Task<bool> RestoreSession(string session)
        {
            string url = HttpService.Url;
            try
            {
                string json = JsonConvert.SerializeObject(session);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await HttpService.Instance.PostAsync(url + "Session/Restore", content);

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

        public async Task<string> CreateSession()
        {
            string url = HttpService.Url;
            try
            {
                var result = await HttpService.Instance.GetAsync(url + "Session/GetSession");

                if (result.IsSuccessStatusCode)
                {
                    return result.Content.ReadAsStringAsync().Result;
                }

                return null;
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
                return null;
            }
        }
    }
}