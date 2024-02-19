using System.Security.Claims;

namespace exercise.wwwapi.Helpers
{
    public static class ClaimsPrincipalHelper
    {
        public static string? Email(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
