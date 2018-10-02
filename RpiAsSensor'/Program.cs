using System;

namespace RpiAsSensor
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().RunAsync(args);
        }

        private async void RunAsync(string[] args)
        {
            Console.WriteLine("Starting program...");
            const string identifier = "rpi";

            Console.WriteLine("Reading temperature...");
            var reader = new TemperatureReader();
            var temperature = await reader.GetTemperatureAsync();
            Console.WriteLine("The temperature is: " + temperature);

            Console.WriteLine("Building JSON...");
            var builder = new MessageBuilder() { Identifier = identifier };
            var json = builder.BuildMessage(temperature);
            Console.WriteLine("The JSON is: ");
            Console.WriteLine(json);

            Console.WriteLine("Posting data do API");
            var client = new RpiHttpClient();
            await client.PostData(json);
            Console.WriteLine("End of the program");
        }
    }
}
