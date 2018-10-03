using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RpiAsSensor
{
    public class RpiHttpClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;

        public RpiHttpClient(string baseUrl)
        {
            _baseUrl = baseUrl;

            _client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10.0)
            };
        }

        public async Task<string> GetToken(string username, string password)
        {
            string responseBody = String.Empty;
            string token = String.Empty;

            var credentials = new JObject
            {
                { "username", username },
                { "password", password }
            };

            var response = await _client.PostAsync($"{_baseUrl}/token", new StringContent(credentials.ToString(), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }

            if (responseBody.Contains("token"))
            {
                var json = JObject.Parse(responseBody);
                token = (string)json["token"];
            }

            return token;
        }

        public async Task PostData(string token, string json)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token); 
            await _client.PostAsync($"{_baseUrl}/sensors/specified", new StringContent(json, Encoding.UTF8, "application/json"));
        }
    }
}
