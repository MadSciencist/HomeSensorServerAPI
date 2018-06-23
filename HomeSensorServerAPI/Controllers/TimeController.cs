using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeController : ControllerBase
    {
        [Route("now")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetTime()
        {
            return Ok(DateTime.UtcNow.ToString());
        }
    }
}