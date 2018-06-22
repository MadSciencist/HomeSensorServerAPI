using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomeSensorServerAPI.Logger;
using HomeSensorServerAPI.Repository;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LoggerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Logger
        [HttpGet]
        public IEnumerable<LogEvent> GetLogEvents()
        {
            return _context.LogEvents;
        }

        // GET: api/Logger/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogEvent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var logEvent = await _context.LogEvents.FindAsync(id);

            if (logEvent == null)
            {
                return NotFound();
            }

            return Ok(logEvent);
        }

        // POST: api/Logger
        [HttpPost]
        public async Task<IActionResult> PostLogEvent([FromBody] LogEvent logEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(logEvent.LogLevel == ELogLevel.FRONT_END)
            {
                _context.LogEvents.Add(logEvent);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetLogEvent", new { id = logEvent.Id }, logEvent);
            }

            return BadRequest();
        }
    }
}