using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Utils;
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

            var existingStreamingDevice = await _streamingDeviceRepository.GetByIdAsync(id);
            var userRole = ClaimsPrincipalHelper.GetClaimedUserRole(this.User);
            var userId = ClaimsPrincipalHelper.GetClaimedUserIdentifierInt(this.User);

            if (!(userRole == EUserRole.Admin || userId == existingStreamingDevice.Owner.Id))
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
            var userId = int.Parse(ClaimsPrincipalHelper.GetClaimedUserIdentifier(this.User));
            var user = await _userRepository.GetByIdAsync(userId);

            streamingDevice.Owner = user;

            var createdDevice = await _streamingDeviceRepository.CreateAsync(streamingDevice);

            return CreatedAtAction("PostStreamingDevice", new { id = streamingDevice.Id }, createdDevice);
        }

        // DELETE: api/StreamingDevices/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> DeleteStreamingDevice([FromRoute] int id)
        {
            var streamingDevice = await _streamingDeviceRepository.GetByIdAsync(id);

            if (streamingDevice == null)
            {
                return NotFound();
            }

            var userRole = ClaimsPrincipalHelper.GetClaimedUserRole(this.User);
            var userId = ClaimsPrincipalHelper.GetClaimedUserIdentifierInt(this.User);

            if (!(userRole == EUserRole.Admin || userId == streamingDevice.Owner.Id))
            {
                return Forbid();
            }

            await _streamingDeviceRepository.DeleteAsync(streamingDevice);

            return Ok(new { Action = "DeleteStreamingDevice", id });
        }
    }
}