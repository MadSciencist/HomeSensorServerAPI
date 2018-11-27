using RpiProcesses;
using System;

namespace RpiAsSensor
{
    public class TemperatureReader
    {
        private const string _command = @"/opt/vc/bin/vcgencmd measure_temp";

        public string GetTemperature()
        {
            var process = new RpiProcessHandler();
            var stdOut = process.ExecuteShellCommand(_command);

            return ParseTemperature(stdOut);
        }

        public string ParseTemperature(string stdOut)
        {
            string temperature = string.Empty;

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
