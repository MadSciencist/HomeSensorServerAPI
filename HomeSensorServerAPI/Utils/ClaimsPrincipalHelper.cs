using HomeSensorServerAPI.Models.Enums;
using System;
using System.Linq;
using System.Security.Claims;

namespace HomeSensorServerAPI.Utils
{
    public static class ClaimsPrincipalHelper
    {
        public static bool IsUserAdmin(ClaimsPrincipal principal) => GetClaimedUserRole(principal).ToString() == EUserRole.Admin.ToString();

        public static  bool IsUserManager(ClaimsPrincipal principal) => GetClaimedUserRole(principal).ToString() == EUserRole.Manager.ToString();

        public static string GetClaimedUserIdentifier(ClaimsPrincipal claimsPrincipal) => claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        public static int GetClaimedUserIdentifierInt(ClaimsPrincipal claimsPrincipal) => int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));

        public static EUserRole GetClaimedUserRole(ClaimsPrincipal claimsPrincipal)
        {
            var claimedRoleString = claimsPrincipal.FindFirstValue(ClaimTypes.Role); //get role
            return Enum.GetValues(typeof(EUserRole)).Cast<EUserRole>().FirstOrDefault(ur => ur.ToString() == claimedRoleString); //and check if it exists in enum
        }
    }
}
