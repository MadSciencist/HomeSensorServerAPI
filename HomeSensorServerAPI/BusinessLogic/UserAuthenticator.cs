using HomeSensorServerAPI.Models;
using HomeSensorServerAPI.Repository;
using System.Linq;

namespace HomeSensorServerAPI.BusinessLogic
{
    public class UserAuthenticator
    {
        AppDbContext _context;
        public UserAuthenticator(AppDbContext context)
        {
            _context = context;
        }

        public User Authenticate(LoginCredentials login)
        {
            User user = null;

            var requestant = _context.Users.FirstOrDefault(u => u.Login == login.Username);

            if (requestant == null)
                return null;

            if (requestant.Password == login.Password)
                user = requestant;

            return user;
        }
    }
}
