using System;
using System.Threading.Tasks;

namespace RpiAsSensor
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting program...");
            new Program().UpdateTemperatureAsync().Wait();
            Console.WriteLine("End of the program");
        }

        public async Task UpdateTemperatureAsync()
        {
            const string baseUrl = @"http://localhost:5000/api";
            const string identifier = "rpi";
            const string username = "homeAutomationSensor";
            const string password = "homeAutomationSensorPassword";

            Console.WriteLine("Reading temperature...");
            var reader = new TemperatureReader();
            var temperature = reader.GetTemperature();
            Console.WriteLine("The temperature is: " + temperature);

            Console.WriteLine("Building JSON...");
            var builder = new MessageBuilder() { Identifier = identifier };
            var json = builder.BuildMessage(temperature);
            Console.WriteLine("The JSON is: ");
            Console.WriteLine(json);


            var client = new RpiHttpClient(baseUrl);
            Console.WriteLine("Getting JWT...");
            string token = await client.GetToken(username, password);
            Console.WriteLine("Got token:");
            Console.WriteLine(token);
            Console.WriteLine("Posting data do API...");
            await client.PostData(token, json);
        }
    }
}
