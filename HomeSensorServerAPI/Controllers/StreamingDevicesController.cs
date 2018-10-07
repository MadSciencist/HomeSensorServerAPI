using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StreamingDevicesController : Controller
    {
        private readonly IStreamingDeviceRepository _streamingDeviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _context;

        public StreamingDevicesController(IStreamingDeviceRepository streamingDeviceRepository, IUserRepository userRepository, AppDbContext context)
        {
            _streamingDeviceRepository = streamingDeviceRepository;
            _userRepository = userRepository;
            _context = context;
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
            if (id != streamingDevice.ID)
            {
                return BadRequest();
            }

            var existingStreamingDevice = _context.StreamingDevices.Include(x => x.Creator).Single(x => x.ID == id);
            var userRole = ClaimsPrincipalHelper.GetClaimedUserRole(this.User);
            var userId = ClaimsPrincipalHelper.GetClaimedUserIdentifierInt(this.User);

            if (!(userRole == EUserRole.Admin || userId == existingStreamingDevice.Creator.ID))
            {
                return Forbid();
            }

            var updatedDevice = await _streamingDeviceRepository.UpdateAsync(streamingDevice);

            return Ok(new { Action = "Update", updatedDevice });
        }

        // POST: api/StreamingDevices
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PostStreamingDevice([FromBody] StreamingDevice streamingDevice)
        {
            StreamingDevice createdDevice = null;
            var userId = int.Parse(ClaimsPrincipalHelper.GetClaimedUserIdentifier(this.User));
            var creator = _context.Users.Single(u => u.ID == userId);

            streamingDevice.Creator = creator;

            try
            {
                createdDevice = await _streamingDeviceRepository.CreateAsync(streamingDevice);
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return CreatedAtAction("PostStreamingDevice", new { id = streamingDevice.ID }, createdDevice);
        }

        // DELETE: api/StreamingDevices/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteStreamingDevice([FromRoute] int id)
        {
            //  var streamingDevice = await _streamingDeviceRepository.GetByIdAsync(id);
            var streamingDevice = _context.StreamingDevices.Include(x => x.Creator).Single(x => x.ID == id);

            if (streamingDevice == null)
            {
                return NotFound();
            }

            var userRole = ClaimsPrincipalHelper.GetClaimedUserRole(this.User);
            var userId = ClaimsPrincipalHelper.GetClaimedUserIdentifierInt(this.User);

            if (!(userRole == EUserRole.Admin || userId == streamingDevice.Creator.ID))
            {
                return Forbid();
            }

            await _streamingDeviceRepository.DeleteAsync(streamingDevice);

            return Ok(new { Action = "DeleteStreamingDevice", id });
        }
    }
}