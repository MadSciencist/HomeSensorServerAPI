using HomeSensorServerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeSensorServerAPI.Utils
{
    public class UserPublicDataProvider
    {
        public IEnumerable<PublicUser> ConvertFullUsersDataToPublicData(IEnumerable<User> users)
        {
            var result = from u in users
                         select new PublicUser()
                         {
                             Name = u.Name,
                             Lastname = u.Lastname,
                             Birthdate = u.Birthdate,
                             Email = u.Email,
                             Role = u.Role,
                             PhotoUrl = u.PhotoUrl,
                             Gender = u.Gender,
                             LastInvalidLogin = u.LastInvalidLogin,
                             LastValidLogin = u.LastValidLogin,
                             JoinDate = u.JoinDate,
                             ID = u.ID,
                             Login = u.Login
                         };

            return result;
        }

        public PublicUser ConvertFullUserDataToPublicData(User user)
        {
            var publicUser = new PublicUser()
            {
                Name = user.Name,
                Lastname = user.Lastname,
                Birthdate = user.Birthdate,
                Login = user.Login,
                Gender = user.Gender,
                PhotoUrl = user.PhotoUrl,
                Role = user.Role,
                Email = user.Email,
                LastInvalidLogin = user.LastInvalidLogin,
                LastValidLogin = user.LastValidLogin,
                JoinDate = user.JoinDate,
                ID = user.ID
            };
            return publicUser;
        }
    }
}
