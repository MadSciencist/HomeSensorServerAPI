using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ServerMvc.Models
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SensorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SensorsController(AppDbContext context)
        {
            _context = context;
        }

        //TODO validate search result, authorize
        //api/sensors/kitchen
        [HttpGet("{identifier}"), AllowAnonymous]
        public IEnumerable<Sensor> Get(string identifier)
        {
            var sensors = _context.Sensors.Where(s => s.Identifier == identifier);

            return sensors;
        }

        //[HttpPut, Authorize]
        [HttpPost]
        //TODO add sensor role claim, refactor URI to more REST style
        [Route("specified")]
        public void Post([FromBody]Sensor sensor)
        {
            if (sensor != null && ModelState.IsValid)
            {
                try
                {
                    if (_context.Nodes.Any(n => n.Identifier == sensor.Identifier))
                    {
                        var actualCount = _context.Sensors.Count(s => s.Identifier == sensor.Identifier);

                        if (actualCount >= 1000) //keep only last 1000 samples
                        {
                            var toRemove = _context.Sensors.Where(s => s.Identifier == sensor.Identifier).OrderBy(s => s.Id).Take(1);
                            _context.Sensors.RemoveRange(toRemove);
                        }
                        _context.Sensors.Add(new Sensor() { Identifier = sensor.Identifier, Data = sensor.Data.ToLower(), TimeStamp = DateTime.Now });

                        _context.SaveChanges();
                    }
                }
                catch (DbUpdateException e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
        }
    }
}