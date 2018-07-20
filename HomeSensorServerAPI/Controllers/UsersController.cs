using HomeSensorServerAPI.BusinessLogic;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using HomeSensorServerAPI.Extensions;
using System.Security.Claims;
using System;
using HomeSensorServerAPI.Logger;
using System.Linq;

namespace LocalSensorServer.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersPublicData()
        {
            IEnumerable<PublicUser> publicData = null;

            var claimedRole = GetClaimedUserRole(this.User);
            if (claimedRole == EUserRole.Admin)
            {
                var users = await _context.Users.ToListAsync();
                var publicDataProvider = new UserPublicDataProvider();
                publicData = publicDataProvider.ConvertFullUsersDataToPublicData(users);
            }
            else
            {
                return Unauthorized();
            }

            return Ok(publicData);
        }

        [HttpGet("{id}")]
        public async Task<PublicUser> GetUserInfo(int id)
        {
            PublicUser publicUser = null;

            if (IsUserIdentifierAsClaimed(id))
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                if (user != null)
                    publicUser = new UserPublicDataProvider().ConvertFullUserDataToPublicData(user);
                else
                    publicUser = new PublicUser();
            }

            return publicUser;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]User candidate)
        {
            //TODO update more variables than this
            if (ModelState.IsValid)
            {
                if (IsUserIdentifierAsClaimed(id) || IsUserAdmin())
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

                    user.Name = candidate.Name;
                    user.Gender = candidate.Gender;
                    user.Lastname = candidate.Lastname;
                    user.PhotoUrl = candidate.PhotoUrl;
                    user.Birthdate = candidate.Birthdate;
                    user.Email = candidate.Email;

                    //prevent null reference exception, because we dont keep user password at front end due to security reasons
                    if(candidate.Password != null)
                    {
                        user.Password = new PasswordCryptoSerivce().CreateHashString(candidate.Password);
                    }

                    //prevent non-privilladged user to change his status to admin by i.e fiddler request
                    if (IsUserAdmin())
                    {
                        user.Role = candidate.Role;
                    }

                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        new LogService().LogToDatabase(_context, e);
                    }

                    return Ok(user);
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            //TODO update more variables than this
            if (ModelState.IsValid)
            {
                if (IsUserIdentifierAsClaimed(id) || IsUserAdmin())
                {
                    var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();

                    return Ok();
                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        private bool IsUserIdentifierAsClaimed(int requestedId)
        {
            var claimedUserIdentifier = GetClaimedUserIdentifier(this.User);

            if (Int32.TryParse(claimedUserIdentifier, out int claimedUserId))
            {
                return claimedUserId == requestedId;
            }
            else
            {
                return false;
            }
        }
        private bool IsUserAdmin()
        {
            return GetClaimedUserRole(this.User).ToString() == EUserRole.Admin.ToString();
        }

        private string GetClaimedUserIdentifier(ClaimsPrincipal claimsPrincipal) => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);

        private EUserRole GetClaimedUserRole(ClaimsPrincipal claimsPrincipal)
        {
            var claimedRoleString = claimsPrincipal.FindFirstValue(ClaimTypes.Role); //get role
            return Enum.GetValues(typeof(EUserRole)).Cast<EUserRole>().FirstOrDefault(ur => ur.ToString() == claimedRoleString); //and check if it exists in enum
        }
    }
}