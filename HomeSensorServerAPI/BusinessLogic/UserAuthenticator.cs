using HomeSensorServerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeSensorServerAPI.BusinessLogic
{
    public class UserAuthenticator
    {
        public User Authenticate(IEnumerable<User> users, LoginCredentials login)
        {
            User user = null;

            var requestant = users.FirstOrDefault(u => u.Login == login.Username);

            if (requestant == null)
                return null;

            if (requestant.Password == login.Password)
                user = requestant;

            return user;
        }
    }
}
