using HomeSensorServerAPI.Logger;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.BusinessLogic
{
    public class CallDeviceApi
    {
        public static async Task<string> SendRequestAsync(Uri deviceUri, string device, string state)
        {
            HttpClient client = new HttpClient();
            string content = null;

            HttpResponseMessage response = null;
            client.Timeout = TimeSpan.FromSeconds(5.0d);
            client.BaseAddress = deviceUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );

            var request = new HttpRequestMessage(HttpMethod.Post, "");

            var requestContent = string.Format("{0}={1}", Uri.EscapeDataString(device), Uri.EscapeDataString(state));

            request.Content = new StringContent(requestContent, Encoding.UTF8, "application/x-www-form-urlencoded");

            try
            {
                response = await client.SendAsync(request);
            }
            catch (HttpRequestException e)
            {
                new LogService().LogToConsole(e);
            }
            catch (Exception e)
            {
                new LogService().LogToConsole(e);
                //TODO add exception handling (timeout)
            }

            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStringAsync();
            }
            else
            {
                content = "timeout";
            }

            return content;
        }
    }
}
