using System.Security.Claims;

namespace exercise.wwwapi.Helpers
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Fetches the primary key identifier of a user from claims.
        /// </summary>
        /// <param name="user">Logged in user</param>
        /// <returns>Primary ID string for the logged-in user</returns>
        public static string? UserId(this ClaimsPrincipal user)
        {
            IEnumerable<Claim> claims = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier);
            return claims.ElementAt(1).Value;
        }
    }
}
