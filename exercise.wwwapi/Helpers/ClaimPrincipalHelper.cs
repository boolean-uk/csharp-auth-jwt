using System.Security.Claims;

namespace exercise.wwwapi.Helpers
{
    public static class ClaimsPrincipalHelper
    {
        /*public static string? UserId(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }*/

        public static string UserId(this ClaimsPrincipal user)
        {
            IEnumerable<Claim> claims = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier);
            return claims.Count() >= 2 ? claims.ElementAt(1).Value : null;

        }

        /*httpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Skip(1) // First is schema id
                .Select(c => c.Value)
                .FirstOrDefault(); // Null if not found*/

        public static string? Email(this ClaimsPrincipal user)
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
