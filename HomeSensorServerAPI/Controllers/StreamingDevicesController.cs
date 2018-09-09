using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StreamingDevicesController : Controller
    {
        private readonly IStreamingDeviceRepository _streamingDeviceRepository;

        public StreamingDevicesController(IStreamingDeviceRepository streamingDeviceRepository)
        {
            _streamingDeviceRepository = streamingDeviceRepository;
        }

        // GET: api/StreamingDevices
        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpGet]
        public IEnumerable<StreamingDevice> GetStreamingDevices()
        {
            return _streamingDeviceRepository.GetAll();
        }

        // GET: api/StreamingDevices/5
        [Authorize(Roles = "Admin,Manager,Viewer")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStreamingDevice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var streamingDevice = await _streamingDeviceRepository.GetByIdAsync(id);

            if (streamingDevice == null)
                return NotFound();

            return Ok(streamingDevice);
        }

        // PUT: api/StreamingDevices/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PutStreamingDevice([FromRoute] int id, [FromBody] StreamingDevice streamingDevice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != streamingDevice.Id)
                return BadRequest();

            var updatedDevice = await _streamingDeviceRepository.UpdateAsync(streamingDevice);

            return Ok(new { Action = "Update", updatedDevice });
        }

        // POST: api/StreamingDevices
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PostStreamingDevice([FromBody] StreamingDevice streamingDevice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDevice = await _streamingDeviceRepository.CreateAsync(streamingDevice);

            return CreatedAtAction("PostStreamingDevice", new { id = streamingDevice.Id }, createdDevice);
        }

        // DELETE: api/StreamingDevices/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteStreamingDevice([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var streamingDevice = await _streamingDeviceRepository.GetByIdAsync(id);

            if (streamingDevice == null)
                return NotFound();

            await _streamingDeviceRepository.DeleteAsync(streamingDevice);

            return Ok(new { Action = "DeleteStreamingDevice", id });
        }
    }
}