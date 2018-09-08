using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Utils;
using System.Linq;
using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using HomeSensorServerAPI.BusinessLogic;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        readonly IConfiguration _config;
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;
        public TokenController(IConfiguration config, AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _config = config;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody]LoginCredentials requestant)
        {
            IActionResult response = BadRequest();

            if (ModelState.IsValid)
            {
                var users = _context.Users.ToList();

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

        private IActionResult CreateResponseToken(IUser user, string tokenString)
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

        private async Task UpdateLastLoginInfo(IUser user)
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

                }
            }
        }
    }
}