using HomeSensorServerAPI.BusinessLogic;
using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocalSensorServer.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<PublicUser>> GetAllUsersPublicData()
        {
            var users = await _context.Users.ToListAsync();

            return new UserPublicDataProvider().ConvertFullUsersDataToPublicData(users);
        }

        [HttpGet("{id}")]
        public async Task<PublicUser> GetUserInfo(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            return new UserPublicDataProvider().ConvertFullUserDataToPublicData(user);
        }
    }
}