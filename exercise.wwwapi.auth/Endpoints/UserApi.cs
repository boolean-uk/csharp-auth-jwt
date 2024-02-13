using exercise.wwwapi.auth.DTOs;
using exercise.wwwapi.auth.Repositories;
using System.Web.Http;

namespace exercise.wwwapi.auth.Endpoints
{
    public static class UserApi
    {
        public static void ConfigureUserApi(this WebApplication app)
        {
            var userGroup = app.MapGroup("/users");
            userGroup.MapGet("/", GetUserInformation);
        }

        [Authorize(Roles = "Admin, Moderator, User")]
        private static IResult GetUserInformation(HttpContext context)
        {
            var user = context.User.GetUserId();
            if (user == null)
            {
                return TypedResults.NotFound();
            }

            var tmp = context.User.GetUserName();
            return TypedResults.Ok(new UserDTO(user));
        }

        [Authorize(Roles = "Admin")]
        public static IResult DeleteUser(HttpContext context)
        {
            

            throw new NotImplementedException();
        }
    }
}
