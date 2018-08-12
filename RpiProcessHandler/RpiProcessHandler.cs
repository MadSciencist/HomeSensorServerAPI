using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RpiProcessHandler
{
    //TODO: 
    //some exception handling
    public class RpiProcessHandler
    {
        public void ExecuteShellCommand(string command)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("You cannot start this process on development machine.");
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
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

                process.Start();
            }
        }
    }
}