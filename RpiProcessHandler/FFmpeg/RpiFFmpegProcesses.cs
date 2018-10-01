using RpiProcesses.FFmpeg;

namespace RpiProcesses
{
    public class RpiFFmpegProcesses : RpiProcessHandler
    {
        public string KillFFmpeg { get; } = @"sudo killall ffmpeg";

        public async void FFmpegStartStreaming(string rtspUrl, EResolution resolution)
        {
            if (rtspUrl == null)
                return;

            var resolutionString = Resolution.GetResolution(resolution);

            var command = $@"ffmpeg -i {rtspUrl} -vcodec libx264 -acodec aac -f flv -r 25 -s {resolutionString} rtmp://localhost/show/stream";
            var rpi = new RpiProcessHandler();
            await rpi.ExecuteShellCommand(command);
        }

        public async void FFmpegStopStreaming()
        {
            var rpi = new RpiProcessHandler();
            await rpi.ExecuteShellCommand(KillFFmpeg);
        }
    }
}
