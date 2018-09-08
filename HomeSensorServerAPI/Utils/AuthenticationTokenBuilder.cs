using HomeSensorServerAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeSensorServerAPI.Utils
{
    public class AuthenticationTokenBuilder
    {
        readonly IConfiguration _config;
        public AuthenticationTokenBuilder(IConfiguration config)
        {
            _config = config;
        }
        public string BuildToken(IUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthenticationJwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["AuthenticationJwt:Issuer"],
              _config["AuthenticationJwt:Issuer"], //this is actually audience
              claims: claims,
              expires: DateTime.Now.AddMinutes(double.Parse(_config["AuthenticationJwt:ValidTime"])),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
