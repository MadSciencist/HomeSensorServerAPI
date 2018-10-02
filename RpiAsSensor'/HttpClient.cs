using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RpiAsSensor
{
    public class RpiHttpClient
    {
        private readonly HttpClient _client;

        public RpiHttpClient()
        {
            _client = new HttpClient();
            PresetClient(_client);
        }

        public async Task<string> PostData(string data)
        {
            HttpResponseMessage response = null;
            string responseContent = null;
            var request = CreateRequest(data);

            try
            {
                response = await _client.SendAsync(request);
            }
            catch (HttpRequestException)
            {
                throw new HttpRequestException("No such host - device might be off");
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException("Device is found, but it is not responding");
            }

            if (response.IsSuccessStatusCode)
            {
                responseContent = await response.Content.ReadAsStringAsync();
            }

            return responseContent;
        }

        private void PresetClient(HttpClient client)
        {
            //client.BaseAddress = new Uri("localhost");
            client.BaseAddress = new Uri("http://192.168.0.223/sensors/specified");
            client.Timeout = TimeSpan.FromSeconds(5.0d);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        private HttpRequestMessage CreateRequest(string urlQuery)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(urlQuery, Encoding.UTF8, "application/json")
            };

            return request;
        }
    }
}
