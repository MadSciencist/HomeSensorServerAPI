﻿using HomeSensorServerAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HomeSensorServerAPI.BusinessLogic
{
    public class AuthenticationTokenBuilder
    {
        readonly IConfiguration _config;
        public AuthenticationTokenBuilder(IConfiguration config)
        {
            _config = config;
        }
        public string BuildToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            claims.Add(new Claim(ClaimTypes.Role, user.Role.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["AuthenticationJwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["AuthenticationJwt:Issuer"],
              _config["AuthenticationJwt:Issuer"], //this is actually audience
              claims: claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}