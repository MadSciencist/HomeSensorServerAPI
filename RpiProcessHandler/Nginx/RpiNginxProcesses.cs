namespace RpiProcessHandler
{
    public class RpiNginxProcesses : RpiProcessHandler
    {
        public string NginxStartCommand { get; set; } = @"sudo /usr/local/nginx/sbin/nginx";
        public string NginxStopCommand { get; set; } = @"sudo /usr/local/nginx/sbin/nginx -s stop";

        public void StartNginx()
        {
            var nginxProcess = new RpiProcessHandler();
            nginxProcess.ExecuteShellCommand(NginxStartCommand);
        }

        public void StopNginx()
        {
            var nginxProcess = new RpiProcessHandler();
            nginxProcess.ExecuteShellCommand(NginxStopCommand);
        }

        public void RestartNginx()
        {
            var nginxProcess = new RpiProcessHandler();
            nginxProcess.ExecuteShellCommand(NginxStopCommand);
            nginxProcess.ExecuteShellCommand(NginxStartCommand);
        }
    }
}
