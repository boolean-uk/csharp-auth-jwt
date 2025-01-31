using System.Security.Claims;

namespace exercise.wwwapi.Helpers
{
    public static class ClaimsPrincipalHelper
    {
        public static int? UserRealId(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Sid);
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }
            return null;
        }

        public static string? UserId(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier); 
            return claim?.Value; 
        }

        public static string? Email(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
        public static string? Role(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Role);
            return claim?.Value;
        }

    }
}
