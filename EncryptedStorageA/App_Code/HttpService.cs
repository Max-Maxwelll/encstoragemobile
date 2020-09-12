using System.Collections.Generic;
using System.Net;
using System.Net.Http;

namespace EncryptedStorageA.App_Code
{
    public class HttpService
    {
        public static string Url { get; } = "http://192.168.43.217:80/";
        public static HttpClient Instance { get; private set; } = new HttpClient();
        internal static Dictionary<string, string> TempData { get; set; }
        public static void Clear()
        {
            Instance = new HttpClient();
        }
    }
}