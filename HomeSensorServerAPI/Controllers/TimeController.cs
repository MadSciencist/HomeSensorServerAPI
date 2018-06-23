using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        [Route("now")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetTime()
        {
            return Ok(DateTime.Now.ToString());
        }

        [Route("utcnow")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetUtcTime()
        {
            return Ok(DateTime.UtcNow.ToString());
        }
    }
}