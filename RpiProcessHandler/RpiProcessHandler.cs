using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RpiProcesses
{
    public class RpiProcessHandler : IRpiProcessHandler
    {
        public string ExecuteShellCommand(string command)
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
                        RedirectStandardOutput = true
                    },
                };
                stdOutput = TryToRunProcess(process);
            }
            else
            {
                Console.WriteLine("You cannot start this process on development machine.");
            }

            return stdOutput;
        }

        private string TryToRunProcess( Process process)
        {
            string stdOutput = string.Empty;

            try
            {
                process.Start();
                stdOutput = process.StandardOutput.ReadToEnd();
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