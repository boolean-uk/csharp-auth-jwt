using System.Security.Claims;
using System.Security.Principal;

namespace Authentication.Helpers
{
    public static class ClaimsPrincipalHelpers
    {
        public static string? UserId(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserId(this ClaimsPrincipal user)
        {
            IEnumerable<Claim> nameIdentifierClaims = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier);

            if (nameIdentifierClaims.Count() >= 2)
            {
                return nameIdentifierClaims.ElementAt(1).Value;
            }

            Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string? UserRole(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Role);
            return claim?.Value;
        }
    }
}