using System.Security.Claims;

namespace exercise.wwwapi.Helpers
{
    public static class ClaimsPrincipalHelper
    {
        public static int? UserRealId(this ClaimsPrincipal user)
        {
            Claim? claim = user.FindFirst(ClaimTypes.Sid);
            return int.Parse(claim?.Value);
        }

    }
}
