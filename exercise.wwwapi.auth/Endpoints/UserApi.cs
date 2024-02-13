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
            userGroup.MapDelete("/admin", DeleteUser);
            userGroup.MapDelete("/", DeleteOwnAccount);
        }

        /// <summary>
        /// Gets the currently authorized users information (Need to be authorized)
        /// </summary>
        /// <param name="context"></param> https://learn.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8.1
        /// <returns></returns> 200 ok if successfull, else 404
        [Authorize(Roles = "Admin, Moderator, User")]
        private static IResult GetUserInformation(HttpContext context)
        {
            var user = context.User.GetUserId();
            if (user == null)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.Ok(user);
        }

        /// <summary>
        /// only a logged in admin who has access to this function can delete any user.
        /// </summary>
        /// <param name="context"></param> https://learn.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8.1
        /// <param name="userManager"></param> is the class from Identity that is used to access the users table that was generated
        /// <param name="userId"></param> the user identifyer that we want o remove from the db
        /// <returns></returns> 200 ok if user exeisted and got removed, 401 if not admin, 403 if userId is wrong
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

        /// <summary>
        /// Only a logged in user can delete their "account" voluntarily
        /// </summary>
        /// <param name="context"></param> https://learn.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8.1
        /// <param name="userManager"></param> is the class from Identity that is used to access the users table that was generated
        /// <returns></returns> 403 if not logged in, 404 if the user for some reason does not exist, 200 ok if user got removed successfully 
        [Authorize(Roles = "User")]
        public static IResult DeleteOwnAccount(HttpContext context, UserManager<ApplicationUser> userManager)
        {
            var user = context.User.GetUserId();
            if (user == null)
            {
                return TypedResults.BadRequest();
            }
            var result = userManager.FindByIdAsync(user).Result;
            if (result == null)
            {
                return TypedResults.NotFound();
            }

            var res = userManager.DeleteAsync(result).Result;

            if (res.Succeeded)
            {
                return TypedResults.Ok($"You are no longer with us");
            }
            throw new NotImplementedException();
        }
    }
}
