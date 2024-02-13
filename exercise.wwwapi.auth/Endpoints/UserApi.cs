using exercise.wwwapi.auth.DTOs;
using exercise.wwwapi.auth.Models;
using exercise.wwwapi.auth.Repositories;
using Microsoft.AspNetCore.Identity;
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
        public static IResult DeleteUser(HttpContext context, UserManager<ApplicationUser> userManager, string userId)
        {
            var currentUser = context.User;
            if (!currentUser.IsInRole("Admin"))
            {
                return TypedResults.Unauthorized();
            }

            if (string.IsNullOrEmpty(userId))
            {
                return TypedResults.BadRequest("User Id is required");
            }

            var deletingUser = userManager.FindByIdAsync(userId).Result;
            if (deletingUser == null)
            {
                return TypedResults.NotFound($"User with id {userId} not found");
            }

            var result = userManager.DeleteAsync(deletingUser).Result;
            if (result.Succeeded)
            {
                return TypedResults.Ok($"User with id {userId} has been deleted from the database");
            } else
            {
                return TypedResults.BadRequest($"Error deelteing user with id {userId}, ");
            }

        }
    }
}
