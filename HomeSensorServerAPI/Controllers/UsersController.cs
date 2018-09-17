using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Models.Enums;
using HomeSensorServerAPI.PasswordCryptography;
using HomeSensorServerAPI.Repository;
using HomeSensorServerAPI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LocalSensorServer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public IActionResult GetAllUsersPublicData()
        {
            var users = _userRepository.GetAll();
            var publicDataProvider = new UserPublicDataProvider();
            var publicData = publicDataProvider.ConvertFullUsersDataToPublicData(users);

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

        [Authorize(Roles = "Admin,Manager")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]User candidate)
        {
            IActionResult response = null;

            if (IsUserIdentifierAsClaimed(id) || ClaimsPrincipalHelper.IsUserAdmin(User) || ClaimsPrincipalHelper.IsUserManager(User))
            {
                var user = await _userRepository.GetByIdAsync(id);

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
                if (ClaimsPrincipalHelper.IsUserAdmin(User) && !IsAdminTryingToDeleteOrRemovePrivilegesItself(id))
                {
                    user.Role = candidate.Role;
                }

                await _userRepository.UpdateAsync(user);

               response =  Ok(new { Action = "Update", user });
            }
            else
            {
                response =  Forbid();
            }

            return response;
        }

        //api/users/new
        [Route("new")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]User user)
        {
            var cryptoService = new PasswordCryptoSerivce();
            user.JoinDate = DateTime.Now;
            user.LastInvalidLogin = DateTime.Now;
            user.Role = EUserRole.NotAproved;
            user.Password = cryptoService.CreateHashString(user.Password);

            await _userRepository.CreateAsync(user);
            return CreatedAtAction("RegisterUser", new { id = user.Id }, user);
        }

        //api/users/delete/id
        [Authorize(Roles = "Admin,Manager")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            IActionResult response = null;

            if (IsUserIdentifierAsClaimed(id) || ClaimsPrincipalHelper.IsUserAdmin(User)) //user can delete only its account, admin can delete all accounts
            {
                if (!IsAdminTryingToDeleteOrRemovePrivilegesItself(id))
                {
                    var user = await _userRepository.GetByIdAsync(id);
                    await _userRepository.DeleteAsync(user);
                    response = Ok(new { Action = "Delete", user });
                }
                else
                {
                    response = BadRequest("Admin cannot delete itself, but can be delete by other admin.");
                }
            }
            else
            {
                response = Unauthorized();
            }

            return response;
        }

        private bool IsAdminTryingToDeleteOrRemovePrivilegesItself(int removedUserId)
        {
            if (ClaimsPrincipalHelper.IsUserAdmin(User))
            {
                string adminIdString = ClaimsPrincipalHelper.GetClaimedUserIdentifier(this.User);

                if (Int32.TryParse(adminIdString, out int adminId))
                    return removedUserId == adminId;
            }

            return false;
        }

        private bool IsUserIdentifierAsClaimed(int requestedId)
        {
            var claimedUserIdentifier = ClaimsPrincipalHelper.GetClaimedUserIdentifier(this.User);

            if (Int32.TryParse(claimedUserIdentifier, out int claimedUserId))
                return claimedUserId == requestedId;
            else
                return false;
        }

        /* adapter helper functions */

    }
}