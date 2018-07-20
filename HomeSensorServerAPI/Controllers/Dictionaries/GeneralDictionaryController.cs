using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HomeSensorServerAPI.Controllers.Dictionaries
{
    [Route("/api/dictionaries/")]
    [Produces("application/json")]
    [ApiController]
    public class GeneralDictionaryController : ControllerBase
    {
        private readonly AppDbContext _context;
        public GeneralDictionaryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("roles")]
        public IActionResult GetUserRoles()
        {
            return Ok(_context.UserRoles.Select(u => new { u.Value, u.Dictionary }));
        }

        [HttpGet("genders")]
        public IActionResult GetUserGenders()
        {
            return Ok(_context.UserGenders.Select(u => new { u.Value, u.Dictionary }));
        }
    }
}
