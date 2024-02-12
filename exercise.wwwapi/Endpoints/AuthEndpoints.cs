using exercise.wwwapi.DTO;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Identity;
using System.IO.Pipes;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var taskGroup = app.MapGroup("auth");
            taskGroup.MapPost("/register", Register);
            taskGroup.MapPost("/login", Login);
        }

        public static async Task<IResult> Register(UserManager<ApplicationUser> userManager, RegisterDTO payload)
        {
            if (payload.userName is null) return TypedResults.BadRequest("Username is required");
            if (payload.email is null) return TypedResults.BadRequest("Email is required");
            if (payload.password is null) return TypedResults.BadRequest("Password is required");
            var result = await userManager.CreateAsync(new ApplicationUser { UserName = payload.userName, Email = payload.email, Role = UserRole.User }, payload.password!);
            if (result.Succeeded)
            {
                return TypedResults.Created("/auth/", result);
            }

            return TypedResults.BadRequest(result.Errors);
        }
        public static IResult Login(IBlogRepository repository)
        {
            return TypedResults.Ok();
        }
    }
}
