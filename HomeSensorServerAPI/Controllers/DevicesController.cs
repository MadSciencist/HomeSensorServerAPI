using HomeSensorServerAPI.BusinessLogic;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly AppDbContext _context;

        public DevicesController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Manager,Viewer")]
        [Route("set")]
        [HttpPost]
        public async Task<IActionResult> SetDeviceStateAsync(int id, int subId, string value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var selectedDevice = _context.Nodes.FirstOrDefault(n => n.Id == id);
            var deviceUri = new Uri("http://" + selectedDevice.IpAddress);

            selectedDevice.IsOn = SingleRelay.GetNewState(value); //write new state
            _context.Entry(selectedDevice).State = EntityState.Modified; //set modified flag
            await _context.SaveChangesAsync();

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                Console.WriteLine();
            }

            string deviceResponse = null;

            try
            {
                deviceResponse = await CallDeviceApi.SendRequestAsync(deviceUri, selectedDevice.Identifier, value);
            }
            catch (Exception e) //this might be nullreferenceexpcetion, but its actually caused by timeout - need to refactor
            {
                return BadRequest("Timeout, Exception: " + e.Message.ToString());
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
        public IActionResult GetDeviceState(string identifier)
        {
            var device = _context.Nodes.FirstOrDefault(n => n.Identifier == identifier);

            if (device == null)
            {
                return BadRequest();
            }
            return Ok(device.IsOn);
        }
    }
}