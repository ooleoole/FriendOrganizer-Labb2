using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace FriendOrganizer.DataAccess
{
   public abstract class HttpClientBase
    {
        protected readonly HttpClient HttpClient;

        protected HttpClientBase(string basePath)
        {
            HttpClient = new HttpClient();
            SetupClient(basePath);
        }
        private void SetupClient(string basePath)
        {
           
            HttpClient.BaseAddress = new Uri(basePath);
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.Timeout = TimeSpan.FromSeconds(30);

        }
    }
}
