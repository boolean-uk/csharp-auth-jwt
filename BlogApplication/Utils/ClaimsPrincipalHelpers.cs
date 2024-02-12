using System.Security.Claims;
using System.Security.Principal;

namespace BlogApplication.Utils
{
    public static class ClaimsPrincipalHelpers
    {
        public static string? UserId(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string? UserRole(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Role);
            return claim?.Value;
        }

        public static string? UserEmail(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }

        // public static string? UserId(this IIdentity identity)
        // {
        //   if (identity != null && identity.IsAuthenticated)
        //   {
        //     // return Guid.Parse(((ClaimsIdentity)identity).Claims.Where(x => x.Type == "NameIdentifier").FirstOrDefault()!.Value);
        //     return ((ClaimsIdentity)identity).Claims.Where(x => x.Type == "NameIdentifier").FirstOrDefault()!.Value;
        //   }
        //   return null;
        // }
    }

}
