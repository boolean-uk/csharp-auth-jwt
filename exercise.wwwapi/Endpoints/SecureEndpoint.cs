using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class SecureEndpoint
    {
        public static void SecureEndpointConfiguration(this WebApplication app)
        {
            var secure = app.MapGroup("secure");
            app.MapGet("/message", GetMessage);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetMessage(IRepository<User> service, ClaimsPrincipal user)
        {
            return TypedResults.Ok($"you are logged in and you are userid: {user.UserRealId().ToString()}!");
        }
    }
}
