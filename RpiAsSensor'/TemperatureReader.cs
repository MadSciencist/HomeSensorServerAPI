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
            var process = new RpiProcessHandler();
            var stdOut = await process.ExecuteShellCommand(_command);

            return ParseTemperature(stdOut);
        }

        public string ParseTemperature(string stdOut)
        {
            string temperature = String.Empty;

            try
            {
                temperature = stdOut.Split('=', '\'')[1];
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return temperature;
        }
    }
}
