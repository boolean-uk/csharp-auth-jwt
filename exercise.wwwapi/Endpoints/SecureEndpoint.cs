using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.EndPoints
{
    public static class SecureApi
    {
        public static void ConfigureSecureEndpoint(this WebApplication app)
        {
            app.MapGet("message", GetMessage);


        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetMessage(IUserRepository service, ClaimsPrincipal user)
        {
            return TypedResults.Ok($"you are logged in and you are userid: {user.UserRealId().ToString()}!");
        }
    }
}
