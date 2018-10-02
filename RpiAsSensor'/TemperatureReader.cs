using RpiProcesses;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RpiAsSensor
{
    public class TemperatureReader
    {
        private const string _command = @"/opt/vc/bin/vcgencmd measure_temp";

        public async Task<string> GetTemperatureAsync()
        {
            Console.WriteLine("In get temp async");
            var process = new RpiProcessHandler();
            Console.WriteLine("created process handler");
            var stdOut = await process.ExecuteShellCommand(_command);

            return ParseTemperature(stdOut);
        }

        public string ParseTemperature(string stdOut)
        {
            string temperature = string.Empty;
            Console.WriteLine("Parsing temperature (in func)...");

            try
            {
                temperature = stdOut.Split('=', '\'')[1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return temperature;
        }
    }
}
