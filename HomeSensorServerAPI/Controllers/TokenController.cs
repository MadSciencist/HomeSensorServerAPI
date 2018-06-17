using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.BusinessLogic;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public TokenController(IConfiguration config, AppDbContext context)
        {
            _context = context;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateToken([FromBody]LoginCredentials requestant)
        {
            var a = Request;

            IActionResult response = BadRequest();

            if (ModelState.IsValid)
            {
                response = Unauthorized();

                var authenticator = new UserAuthenticator(_context);
                var user = authenticator.Authenticate(requestant);

                if (user != null)
                {
                    var builder = new AuthenticationTokenBuilder(_config);
                    var tokenString = builder.BuildToken(user);
                    response = Json(new { token = tokenString, userId = user.Id });
                }
            }
            return response;
        }
    }
}