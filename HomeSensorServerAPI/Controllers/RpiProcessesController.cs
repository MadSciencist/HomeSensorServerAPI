using HomeSensorServerAPI.Models;
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
        [HttpPost("ffmpeg/start")]
        public IActionResult StartFfmpeg([FromBody] FFmpegProcessEndpoint ffmpeg)
        {
            if (ModelState.IsValid)
            {
                var rtspUrl = ffmpeg.Url;

                //todo parse resolution
                var rpi = new RpiFFmpegProcesses();
                rpi.FFmpegStartStreaming(rtspUrl, EResolution.Res640x480);

                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("ffmpeg/stop")]
        public IActionResult StopFFmpeg()
        {
            var rpi = new RpiFFmpegProcesses();
            rpi.FFmpegStopStreaming();

            return Ok();
        }
    }
}