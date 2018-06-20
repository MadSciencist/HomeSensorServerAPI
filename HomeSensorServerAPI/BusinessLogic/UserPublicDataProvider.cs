using HomeSensorServerAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeSensorServerAPI.BusinessLogic
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
                             LastValidLogin = u.LastValidLogin
                         };

            return result;
        }

        public PublicUser ConvertFullUserDataToPublicData(User user)
            => new PublicUser()
            {
                Name = user.Name,
                Lastname = user.Lastname,
                Birthdate = user.Birthdate,
                Gender = user.Gender,
                PhotoUrl = user.PhotoUrl,
                Role = user.Role,
                Email = user.Email,
                LastInvalidLogin = user.LastInvalidLogin,
                LastValidLogin = user.LastValidLogin                
            };
    }
}
