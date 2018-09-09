using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeSensorServerAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TimeController : Controller
    {
        [Route("now")]
        [HttpGet]
        public IActionResult GetTime()
        {
            return Ok(DateTime.Now.ToString());
        }

        [Route("now/utc")]
        [HttpGet]
        public IActionResult GetUtcTime()
        {
            return Ok(DateTime.UtcNow.ToString());
        }
    }
}