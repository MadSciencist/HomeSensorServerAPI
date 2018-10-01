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
            const string identifier = "rpi";

            var reader = new TemperatureReader();
            var temperature = await reader.GetTemperatureAsync();

            var builder = new MessageBuilder() { Identifier = identifier };
            var json = builder.BuildMessage(temperature);

            var client = new RpiHttpClient();
            await client.PostData(json);
        }
    }
}
