using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.BusinessLogic;
using System.Linq;
using System.Collections.Generic;
using System;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
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
            IActionResult response = BadRequest();

            if (ModelState.IsValid)
            {
                IEnumerable<User> users = _context.Users.ToList();

                var authenticator = new UserAuthenticator();
                var user = authenticator.Authenticate(users, requestant);

                if (user != null) //user found
                {
                    var builder = new AuthenticationTokenBuilder(_config);
                    var tokenString = builder.BuildToken(user);
                    response = Json(new
                    {
                        token = tokenString,
                        userId = user.Id,
                        tokenIssueTime = DateTime.UtcNow.ToString(),
                        tokenValidTo = DateTime.UtcNow.AddMinutes(double.Parse(_config["AuthenticationJwt:ValidTime"])).ToString(),
                    });
                }
                else //no matching user
                {
                    response = NotFound("Podany użytkownik nie istnieje");
                }
            }
            return response;
        }
    }
}