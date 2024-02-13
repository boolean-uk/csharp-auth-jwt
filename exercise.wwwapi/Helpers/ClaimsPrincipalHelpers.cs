using System.Security.Principal;
using System.Security.Claims;
public static class ClaimsPrincipalHelpers
{
    // etension methods
    public static string? UserId(this ClaimsPrincipal user)
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier);
        return claim?.Value;
    }
    public static string? UserName(this ClaimsPrincipal user)
    {
        Claim? claim = user.FindFirst(ClaimTypes.Name);
        return claim?.Value;
    }
    public static string? UserEmail(this ClaimsPrincipal user)
    {
        Claim? claim = user.FindFirst(ClaimTypes.Email);
        return claim?.Value;
    }
    public static string? UserRole(this ClaimsPrincipal user)
    {
        Claim? claim = user.FindFirst(ClaimTypes.Role);
        return claim?.Value;
    }
}
