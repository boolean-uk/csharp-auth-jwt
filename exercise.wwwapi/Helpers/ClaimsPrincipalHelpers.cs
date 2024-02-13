using System.Security.Principal;
using System.Security.Claims;

public static class ClaimsPrincipalHelpers
{
    // etension methods
    public static string? UserId(this ClaimsPrincipal user) //define a method userID, attached to the ClaimsPrincipal class, it adds the uderID
                                                            //method to the ClaimsPrincipal class
    {
        Claim? claim = user.FindFirst(ClaimTypes.NameIdentifier); // go to the claims and find the first one that matches the nameidentifier and
                                                                  // retreive it as a claim object
        return claim?.Value; // returns a nullable string, questionmark says to return null if value is null or if value not null retrieve the value
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