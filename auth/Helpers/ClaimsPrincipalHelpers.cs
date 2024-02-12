
using System.Security.Claims;

namespace auth.Helpers
{
    public static class ClaimsPrincipalHelpers
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
        // Get all claims of type NameIdentifier
        IEnumerable<Claim> nameIdentifierClaims = user.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier);

        // Ensure there are at least two claims of this type
        if (nameIdentifierClaims.Count() >= 2)
        {
            // Get the second claim (index 1) and return its value
            return nameIdentifierClaims.ElementAt(1).Value;
        }
        else
        {
            // Handle cases where there aren't two NameIdentifier claims
            return null; // Or throw an exception, log a warning, etc.
        }
    }
    }
}