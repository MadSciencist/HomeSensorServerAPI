using Newtonsoft.Json.Linq;

namespace RpiAsSensor
{
    class MessageBuilder
    {
        public string Identifier { get; set; }

        public string BuildMessage(string message)
        {
            var data = new JObject
            {
                { "temperature", message }
            };

            var json = new JObject
            {
                { "identifier", Identifier },
                { "data", data }
            };

            return json.ToString();
        }
    }
}
