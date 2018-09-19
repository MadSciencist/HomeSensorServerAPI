using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSDeviceCaller;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    public class SetDeviceBindModel
    {
        public string State { get; set; }
    }
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly INodeRepository _nodeRepository;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(INodeRepository nodeRepository, ILogger<DevicesController> logger)
        {
            _nodeRepository = nodeRepository;
            _logger = logger;
        }

        [Authorize(Roles = "Admin,Manager,Viewer")]
        [Route("set/{identifier}")]
        [HttpPost]
        public async Task<IActionResult> SetDeviceStateAsync(string identifier, [FromBody]SetDeviceBindModel state)
        {
            var selectedDevice = await _nodeRepository.GetWithIdentifierAsync(identifier);

            if (selectedDevice == null) return NotFound();

            string deviceResponse = null;
            var device = new Device() { Identifier = selectedDevice.IpAddress, IpAddress = selectedDevice.IpAddress };
            var caller = new DeviceCaller(device);

            //selectedDevice.IsOn  = String.Equals(value, "on") ? true : false;
            //_context.Entry(selectedDevice).State = EntityState.Modified; //set modified flag
            //await _context.SaveChangesAsync();           
            //await _context.SaveChangesAsync();

            try
            {
                deviceResponse = await caller.SetStateAsync(state.State);
            }
            catch (TimeoutException e)
            {
                _logger.LogError(e, "Cannot set device state: not responding");
                return BadRequest("Cannot set device state: not responding");
            }
            catch(HttpRequestException e)
            {
                _logger.LogError(e, "Cannot set device state: not found");
                return BadRequest("Cannot set device state: not found");
            }

            if (deviceResponse != null)
            {
                return Ok(deviceResponse);
            }
            else
            {
                return BadRequest();
            }
        }

        // TODO on ebedded and here
        // implement this on embedded, add subIdentifier if one device has multiple outputs
        [Route("get")]
        [HttpGet]
        public async Task<IActionResult> GetDeviceStateAsync(string identifier)
        {
            var device = await _nodeRepository.GetWithIdentifierAsync(identifier);

            if (device == null)
            {
                return BadRequest();
            }
            return Ok(device.IsOn);
        }
    }
}