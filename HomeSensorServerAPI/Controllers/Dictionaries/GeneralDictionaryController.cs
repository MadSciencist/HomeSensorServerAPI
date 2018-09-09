using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HomeSensorServerAPI.Controllers.Dictionaries
{
    [Authorize]
    [ApiController]
    [Route("/api/dictionaries/")]
    public class GeneralDictionaryController : Controller
    {
        private readonly AppDbContext _context;
        public GeneralDictionaryController(AppDbContext context) =>  _context = context;

        [HttpGet("roles")]
        public IActionResult GetUserRoles()
        {
            return Ok(_context.UserRoles.Select(u => new { u.Key, u.Value }));
        }

        [HttpGet("genders")]
        public IActionResult GetUserGenders()
        {
            return Ok(_context.UserGenders.Select(u => new { u.Key, u.Value }));
        }

        [HttpGet("types/node")]
        public IActionResult GetNodeTypes()
        {
            return Ok(_context.NodeTypes.Select(u => new { u.Key, u.Value }));
        }

        [HttpGet("types/sensor")]
        public IActionResult GetSensorTypes()
        {
            return Ok(_context.SensorTypes.Select(u => new { u.Key, u.Value }));
        }

        [HttpGet("types/actuator")]
        public IActionResult GetActuatorTypes()
        {
            return Ok(_context.ActuatorTypes.Select(u => new { u.Key, u.Value }));
        }
    }
}
