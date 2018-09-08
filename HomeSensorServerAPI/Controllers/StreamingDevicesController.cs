using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamingDevicesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IStreamingDeviceRepository _streamingDeviceRepository;

        public StreamingDevicesController(IStreamingDeviceRepository streamingDeviceRepository, AppDbContext context)
        {
            _context = context;
            _streamingDeviceRepository = streamingDeviceRepository;
        }

        // GET: api/StreamingDevices
        [HttpGet]
        public IEnumerable<StreamingDevice> GetStreamingDevices()
        {
            return _streamingDeviceRepository.GetAll();
        }

        // GET: api/StreamingDevices/5
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
        public async Task<IActionResult> PostStreamingDevice([FromBody] StreamingDevice streamingDevice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdDevice = await _streamingDeviceRepository.CreateAsync(streamingDevice);

            return CreatedAtAction("PostStreamingDevice", new { id = streamingDevice.Id }, createdDevice);
        }

        // DELETE: api/StreamingDevices/5
        [HttpDelete("{id}")]
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