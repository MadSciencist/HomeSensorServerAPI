namespace RpiProcessHandler
{
    public class RpiFFmpegProcesses : RpiProcessHandler
    {
        public string RunFFmpeg { get; set; } = $"ffmpeg -i rtsp://192.168.0.80:554/axis-media/media.amp? -vcodec libx264 -acodec aac -f flv -r 25 -s 640x480 rtmp://localhost/show/stream";

        public void FFmpegStartStreaming()
        {
            var rpi = new RpiProcessHandler();
            rpi.ExecuteShellCommand(RunFFmpeg);
        }
    }
}
