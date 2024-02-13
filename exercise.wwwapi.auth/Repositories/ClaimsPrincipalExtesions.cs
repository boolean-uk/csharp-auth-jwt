using System.Security.Claims;

namespace exercise.wwwapi.auth.Repositories
{
    public static class ClaimsPrincipalExtesions
    {

        public static string? GetUserId(this ClaimsPrincipal user)
        {
            IEnumerable<Claim> claims = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier);

            if (claims.Count() >= 2)
            {
                return claims.ElementAtOrDefault(1).Value;
            }
            return null;
        }

        public static string? GetUserName(this ClaimsPrincipal user)
        {
            IEnumerable<Claim> claims = user.Claims.ToList();
            
            if (claims.Count() >= 4)
            {
                return claims.ElementAtOrDefault(4).Value;
            }

            return null;
        }
    }
}
