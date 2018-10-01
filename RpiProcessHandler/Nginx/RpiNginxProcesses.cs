namespace RpiProcesses
{
    public class RpiNginxProcesses : RpiProcessHandler
    {
        public string NginxStartCommand { get; set; } = @"sudo /usr/local/nginx/sbin/nginx";
        public string NginxStopCommand { get; set; } = @"sudo /usr/local/nginx/sbin/nginx -s stop";

        public async void StartNginx()
        {
            var nginxProcess = new RpiProcessHandler();
            await nginxProcess.ExecuteShellCommand(NginxStartCommand);
        }

        public async void StopNginx()
        {
            var nginxProcess = new RpiProcessHandler();
            await nginxProcess.ExecuteShellCommand(NginxStopCommand);
        }

        public async void RestartNginx()
        {
            var nginxProcess = new RpiProcessHandler();
            await nginxProcess.ExecuteShellCommand(NginxStopCommand);
            await nginxProcess.ExecuteShellCommand(NginxStartCommand);
        }
    }
}
