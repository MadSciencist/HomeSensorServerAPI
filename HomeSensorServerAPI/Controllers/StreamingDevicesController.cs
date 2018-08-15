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

        public StreamingDevicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/StreamingDevices
        [HttpGet]
        public IEnumerable<StreamingDevice> GetStreamingDevices()
        {
            return _context.StreamingDevices;
        }

        // GET: api/StreamingDevices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStreamingDevice([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var streamingDevice = await _context.StreamingDevices.FindAsync(id);

            if (streamingDevice == null)
            {
                return NotFound();
            }

            return Ok(streamingDevice);
        }

        // PUT: api/StreamingDevices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStreamingDevice([FromRoute] long id, [FromBody] StreamingDevice streamingDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != streamingDevice.Id)
            {
                return BadRequest();
            }

            _context.Entry(streamingDevice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StreamingDeviceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StreamingDevices
        [HttpPost]
        public async Task<IActionResult> PostStreamingDevice([FromBody] StreamingDevice streamingDevice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.StreamingDevices.Add(streamingDevice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStreamingDevice", new { id = streamingDevice.Id }, streamingDevice);
        }

        // DELETE: api/StreamingDevices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStreamingDevice([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var streamingDevice = await _context.StreamingDevices.FindAsync(id);
            if (streamingDevice == null)
            {
                return NotFound();
            }

            _context.StreamingDevices.Remove(streamingDevice);
            await _context.SaveChangesAsync();

            return Ok(streamingDevice);
        }

        private bool StreamingDeviceExists(long id)
        {
            return _context.StreamingDevices.Any(e => e.Id == id);
        }
    }
}