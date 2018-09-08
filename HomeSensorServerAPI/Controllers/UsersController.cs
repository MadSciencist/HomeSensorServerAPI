using HomeSensorServerAPI.BusinessLogic;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.PasswordCryptography;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LocalSensorServer.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _userRepository;

        public UsersController(AppDbContext context, IUserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        [HttpGet]
        public IActionResult GetAllUsersPublicData()
        {
            IEnumerable<PublicUser> publicData = null;

            if (GetClaimedUserRole(this.User) == EUserRole.Admin)
            {
                var users = _userRepository.GetAll().ToList();
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
        public async Task<IActionResult> GetUserInfo(int id)
        {
            IActionResult response = null;

            if (IsUserIdentifierAsClaimed(id))
            {
                var user = await _userRepository.GetByIdAsync(id);

                if (user == null)
                {
                    response = NotFound();
                }
                else
                {
                    response = Ok(new UserPublicDataProvider().ConvertFullUserDataToPublicData(user));
                }
            }
            else
            {
                response = Forbid();
            }

            return response;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]User candidate)
        {
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
                    if (candidate.Password != null)
                    {
                        user.Password = new PasswordCryptoSerivce().CreateHashString(candidate.Password);
                    }

                    //prevent non-priviledged user to change his status to admin by i.e fiddler request
                    if (IsUserAdmin() && !IsAdminTryingToDeleteOrRemovePrivilegesItself(id))
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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]User user)
        {
            if (ModelState.IsValid)
            {
                var cryptoService = new PasswordCryptoSerivce();
                user.JoinDate = DateTime.Now;
                user.LastInvalidLogin = DateTime.Now;
                user.Role = EUserRole.NotAproved;
                user.Password = cryptoService.CreateHashString(user.Password);

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return CreatedAtAction("RegisterUser", new { id = user.Id }, user);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (ModelState.IsValid)
            {
                if (IsUserIdentifierAsClaimed(id) || IsUserAdmin()) //user can delete only its account, admin can delete all accounts
                {
                    if (!IsAdminTryingToDeleteOrRemovePrivilegesItself(id))
                    {
                        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                        _context.Users.Remove(user);
                        await _context.SaveChangesAsync();

                        return Ok();
                    }
                    else
                    {
                        BadRequest("Admin cannot delete itself, but can be delete by other admin.");
                    }

                }
                else
                {
                    return Unauthorized();
                }
            }
            return BadRequest();
        }

        private bool IsAdminTryingToDeleteOrRemovePrivilegesItself(int removedUserId)
        {
            if (IsUserAdmin())
            {
                string adminIdString = GetClaimedUserIdentifier(this.User);

                if(Int32.TryParse(adminIdString, out int adminId))
                    return removedUserId == adminId;
            }

            return false;
        }

        private bool IsUserIdentifierAsClaimed(int requestedId)
        {
            var claimedUserIdentifier = GetClaimedUserIdentifier(this.User);

            if (Int32.TryParse(claimedUserIdentifier, out int claimedUserId))
                return claimedUserId == requestedId;
            else
                return false;
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