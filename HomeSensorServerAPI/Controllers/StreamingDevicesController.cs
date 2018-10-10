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

        public StreamingDevicesController(IStreamingDeviceRepository streamingDeviceRepository, IUserRepository userRepository)
        {
            _streamingDeviceRepository = streamingDeviceRepository;
            _userRepository = userRepository;
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
            if (id != streamingDevice.Id)
            {
                return BadRequest();
            }

            var existingStreamingDevice = _streamingDeviceRepository.AsQueryableNoTrack().Include(x => x.Creator).SingleOrDefault(x => x.Id == id);

            if (existingStreamingDevice == null)
            {
                return NotFound();
            }

            var userRole = ClaimsPrincipalHelper.GetClaimedUserRole(this.User);
            var userId = ClaimsPrincipalHelper.GetClaimedUserIdentifierInt(this.User);

            if (!(userRole == EUserRole.Admin || userId == existingStreamingDevice.Creator.Id))
            {
                return Forbid();
            }

            StreamingDevice updatedDevice = null;

            try
            {
                updatedDevice = await _streamingDeviceRepository.UpdateAsync(streamingDevice);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok(new { Action = "Update", updatedDevice });
        }

        // POST: api/StreamingDevices
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> PostStreamingDevice([FromBody] StreamingDevice streamingDevice)
        {
            StreamingDevice createdDevice = null;
            var userRole = ClaimsPrincipalHelper.GetClaimedUserRole(this.User);
            var userId = int.Parse(ClaimsPrincipalHelper.GetClaimedUserIdentifier(this.User));
            var creator = await _userRepository.GetByIdAsync(userId);

            streamingDevice.Creator = creator;

            try
            {
                createdDevice = await _streamingDeviceRepository.CreateAsync(streamingDevice);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return CreatedAtAction("PostStreamingDevice", new { id = streamingDevice.Id }, createdDevice);
        }

        // DELETE: api/StreamingDevices/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteStreamingDevice([FromRoute] int id)
        {
            var streamingDevice = _streamingDeviceRepository.AsQueryable().Include(x => x.Creator).Single(x => x.Id == id);

            if (streamingDevice == null)
            {
                return NotFound();
            }

            var userRole = ClaimsPrincipalHelper.GetClaimedUserRole(this.User);
            var userId = ClaimsPrincipalHelper.GetClaimedUserIdentifierInt(this.User);

            if (!(userRole == EUserRole.Admin || userId == streamingDevice.Creator.Id))
            {
                return Forbid();
            }

            await _streamingDeviceRepository.DeleteAsync(streamingDevice);

            return Ok(new { Action = "DeleteStreamingDevice", id });
        }
    }
}