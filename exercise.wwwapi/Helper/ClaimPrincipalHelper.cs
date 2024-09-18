using System.Security.Claims;

namespace exercise.wwwapi.Helper
{
    public static class ClaimsPrincipalHelper
    {
        public static string AuthorId(this ClaimsPrincipal author)
        {
            IEnumerable<Claim> claims = author.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier);
            return claims.Count() >= 2 ? claims.ElementAt(1).Value : null;

        }
        public static string? Email(this ClaimsPrincipal author)
        {
            Claim? claim = author.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
        public static string? Role(this ClaimsPrincipal author)
        {
            Claim? claim = author.FindFirst(ClaimTypes.Role);
            return claim?.Value;
        }
    }
}
