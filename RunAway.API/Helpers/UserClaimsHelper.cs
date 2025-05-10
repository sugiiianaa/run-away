using System.Security.Claims;
using RunAway.Domain.Enums;

namespace RunAway.API.Helpers
{
    public static class UserClaimsHelper
    {
        public static Guid? GetUserId(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return null;
            }
            return userId;
        }

        public static string? GetUserEmail(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string? GetUserRole(ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value;
        }

        public static bool IsInRole(ClaimsPrincipal user, string role)
        {
            return user.IsInRole(role);
        }

        public static bool IsAdmin(ClaimsPrincipal user)
        {
            return user.IsInRole(UserRoles.Admin.ToString());
        }

        public static bool IsSuperUser(ClaimsPrincipal user)
        {
            return user.IsInRole(UserRoles.SuperUser.ToString());
        }
    }
}
