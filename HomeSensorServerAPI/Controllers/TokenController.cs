using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace HomeSensorServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
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
                var users = _userRepository.GetAll();

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
                    response = Unauthorized();
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
                await _userRepository.UpdateAsync(user as User);
        }
    }
}