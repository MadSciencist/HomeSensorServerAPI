using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace RpiProcesses
{
    public class RpiProcessHandler : IRpiProcessHandler
    {
        public async Task<string> ExecuteShellCommand(string command)
        {
            string stdOutput = string.Empty;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var escapeCommand = command.Replace("\"", "\\\"");

                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = $"-c \"{escapeCommand}\"",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

                stdOutput = await TryToRunProcess(stdOutput, process);

            }
            else
            {
                Console.WriteLine("You cannot start this process on development machine.");
            }

            return stdOutput;
        }

        private async Task<string> TryToRunProcess(string stdOutput, Process process)
        {
            try
            {
                process.Start();
                stdOutput = await process.StandardOutput.ReadToEndAsync();
                Console.WriteLine("STD output: " + stdOutput);
                process.WaitForExit();
                process.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return stdOutput;
        }
    }
}