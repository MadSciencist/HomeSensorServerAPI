﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.BusinessLogic;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class DevicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DevicesController(AppDbContext context)
        {
            _context = context;
        }

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

        // TODO
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