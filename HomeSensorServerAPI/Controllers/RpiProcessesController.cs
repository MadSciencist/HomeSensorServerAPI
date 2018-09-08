using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RpiProcessHandler;
using RpiProcessHandler.FFmpeg;
using RpiProcessHandler.Rtsp;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class RpiProcessesController : ControllerBase
    {
        private readonly IStreamingDeviceRepository _streamingDeviceRepository;

        public RpiProcessesController(IStreamingDeviceRepository streamingDeviceRepository)
        {
            _streamingDeviceRepository = streamingDeviceRepository;
        }

        [Authorize(Roles = "Admin")]
        [Route("nginx/start")]
        [HttpGet]
        public IActionResult StartNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.StartNginx();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("nginx/stop")]
        public IActionResult StopNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.StopNginx();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("nginx/restart")]
        public IActionResult RestartNginx()
        {
            var rpi = new RpiNginxProcesses();
            rpi.RestartNginx();

            return Ok();
        }

        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpPost("ffmpeg/start")]
        public async Task<IActionResult> StartFfmpeg([FromBody] FFmpegProcessEndpoint ffmpeg)
        {
            if (ModelState.IsValid)
            {
                var selectedDevice = await _streamingDeviceRepository.GetByIdAsync(ffmpeg.StreamingDeviceId);
                var rtspWithCredentials = RtspHelper.InjectCredentialsToUrl(selectedDevice.ConnectionString, selectedDevice.Login, selectedDevice.Password);

                //todo parse resolution
                var rpi = new RpiFFmpegProcesses();
                rpi.FFmpegStartStreaming(rtspWithCredentials, EResolution.Res640x480);

                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpPost("ffmpeg/stop")]
        public IActionResult StopFFmpeg()
        {
            var rpi = new RpiFFmpegProcesses();
            rpi.FFmpegStopStreaming();

            return Ok();
        }
    }
}