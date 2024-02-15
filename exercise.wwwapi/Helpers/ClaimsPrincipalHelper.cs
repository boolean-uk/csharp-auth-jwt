using System.Security.Claims;

namespace exercise.wwwapi.Helpers
{
    public static class ClaimsPrincipalHelper
    {
        public static string? UserId(this ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Skip(1)
                .Select(c => c.Value)
                .FirstOrDefault();
        }

        public static string? Email(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
