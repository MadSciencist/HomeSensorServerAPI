using Microsoft.AspNetCore.Mvc;
using RpiProcessHandler;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RpiProcessesController : ControllerBase
    {
        [Route("nginx/start")]
        [HttpGet]
        public IActionResult StartNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.StartNginx();

            return Ok();
        }

        [Route("nginx/stop")]
        [HttpGet]
        public IActionResult StopNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.StopNginx();

            return Ok();
        }

        [Route("nginx/restart")]
        [HttpGet]
        public IActionResult RestartNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.RestartNginx();

            return Ok();
        }

        [Route("ffmpeg/start")]
        [HttpGet]
        public IActionResult StartFfmpeg()
        {
            var rpi = new RpiFFmpegProcesses();
            rpi.FFmpegStartStreaming();

            return Ok();
        }
    }
}