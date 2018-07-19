using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.BusinessLogic;
using System.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HomeSensorServerAPI.Logger;

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
        public async Task<IActionResult> CreateToken([FromBody]LoginCredentials requestant)
        {
            IActionResult response = BadRequest();

            if (ModelState.IsValid)
            {
                IEnumerable<User> users = _context.Users.ToList();

                var authenticator = new UserAuthenticator();
                var user = authenticator.Authenticate(users, requestant);

                await UpdateLastLoginInfo(user);

                if (user != null && user.IsSuccessfullyAuthenticated) //user found
                {
                    var builder = new AuthenticationTokenBuilder(_config);
                    var tokenString = builder.BuildToken(user);

                    response = CreateResponseToken(user, tokenString);
                }
                else //no matching user
                {
                    response = NotFound("Podany użytkownik nie istnieje");
                }
            }
            return response;
        }

        private IActionResult CreateResponseToken(User user, string tokenString)
        {
            return Ok(JsonConvert.SerializeObject(new
            {
                token = tokenString,
                userId = user.Id,
                userRole = user.Role.ToString(),
                tokenIssueTime = DateTime.Now.ToString(),
                tokenValidTo = DateTime.Now.AddMinutes(double.Parse(_config["AuthenticationJwt:ValidTime"])).ToString(),
            }));
        }

        private async Task UpdateLastLoginInfo(User user)
        {
            if (user != null)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch(Exception e)
                {
                    LogException(e);
                }
            }
        }

        private void LogException(Exception e)
        {
            var logger = new LogService();
            logger.LogToDatabase(_context, e);
        }
    }
}