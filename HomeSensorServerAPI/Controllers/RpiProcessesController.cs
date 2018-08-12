using Microsoft.AspNetCore.Mvc;
using RpiProcessHandler;
using RpiProcessHandler.FFmpeg;

namespace HomeSensorServerAPI.Controllers
{
    //TODO Authorization
    [Route("api/[controller]")]
    [ApiController]
    public class RpiProcessesController : ControllerBase
    {
        //ngix -> authorize only for admins
        [Route("nginx/start")]
        [HttpGet]
        public IActionResult StartNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.StartNginx();

            return Ok();
        }

        [HttpGet("nginx/stop")]
        public IActionResult StopNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.StopNginx();

            return Ok();
        }

        [HttpGet("nginx/restart")]
        public IActionResult RestartNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.RestartNginx();

            return Ok();
        }

        //ffmpeg -> authorized for users
        [HttpGet("ffmpeg/start")]
        public IActionResult StartFfmpeg()
        {
            var rtsp = @"rtsp://192.168.0.80:554/axis-media/media.amp";
            var rpi = new RpiFFmpegProcesses();
            rpi.FFmpegStartStreaming(rtsp, EResolution.Res640x480);

            return Ok();
        }
        [HttpGet("ffmpeg/stop")]
        public IActionResult StopFFmpeg()
        {
            var rpi = new RpiFFmpegProcesses();
            rpi.FFmpegStopStreaming();

            return Ok();
        }
    }
}