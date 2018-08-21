using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using RpiProcessHandler;
using System.Linq;
using RpiProcessHandler.FFmpeg;
using RpiProcessHandler.Rtsp;

namespace HomeSensorServerAPI.Controllers
{
    //TODO Authorization
    [Route("api/[controller]")]
    [ApiController]
    public class RpiProcessesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RpiProcessesController(AppDbContext context)
        {
            _context = context;
        }
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
                var selectedDevice = _context.StreamingDevices.FirstOrDefault(d => d.Id == ffmpeg.StreamingDeviceId);
                var rtspWithCredentials = RtspHelper.InjectCredentialsToUrl(selectedDevice.ConnectionString, selectedDevice.Login, selectedDevice.Password);

                //todo parse resolution
                var rpi = new RpiFFmpegProcesses();
                rpi.FFmpegStartStreaming(rtspWithCredentials, EResolution.Res640x480);

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