using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace exercise.wwwapi.Utils
{
    public static class HttpContextHelper
    {
        /// <summary>
        /// Retrieve the Id of the user iteracting with the endpoint.
        /// </summary>
        /// <param name="user">The logged in user interacting with the webserver</param>
        /// <returns>A string of the User ID, if none found returns null</returns>
        public static string? UserId(this ClaimsPrincipal user) 
        {
            return user.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .Skip(1) // First is schema id
                .Select(c => c.Value)
                .FirstOrDefault(); // Null if not found
        }
    }
}
