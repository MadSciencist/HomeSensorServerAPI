using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NSDeviceCaller
{
    public class DeviceCaller : IDeviceCaller
    {
        private readonly Device _device;
        private readonly HttpClient _client;

        public DeviceCaller(Device device)
        {
            _device = device;
            _client = new HttpClient();
            PresetClient(_client, _device);
        }

        public async Task<string> SetStateAsync(string state)
        {
            HttpResponseMessage response = null;
            string responseContent = null;
            var request = CreateRequest(state);

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

        private void PresetClient(HttpClient client, Device device)
        {
            client.BaseAddress = new Uri("http://" + device.IpAddress);
            client.Timeout = TimeSpan.FromSeconds(5.0d);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("text/plain")
            );
        }

        private HttpRequestMessage CreateRequest(string urlQuery)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                Content = new StringContent(urlQuery, Encoding.UTF8, "application/x-www-form-urlencoded")
            };

            return request;
        }
    }
}
