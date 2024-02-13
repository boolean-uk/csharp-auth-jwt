using exercise.wwwapi.DTO;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
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
            if (payload.username is null) return TypedResults.BadRequest("Username is required");
            if (payload.email is null) return TypedResults.BadRequest("Email is required");
            if (payload.password is null) return TypedResults.BadRequest("Password is required");
            var result = await userManager.CreateAsync(new ApplicationUser { UserName = payload.username, Email = payload.email, Role = UserRole.User }, payload.password!);
            if (result.Succeeded)
            {
                return TypedResults.Created("/auth/", new RegisterResponseDTO(payload.username, payload.email, UserRole.User));
            }

            return TypedResults.BadRequest(result.Errors);
        }
        public static async Task<IResult> Login(LoginDTO payload, UserManager<ApplicationUser> userManager, IBlogRepository repository, TokenService tokenService)
        {
            //check payload
            if (payload.username == null) return TypedResults.BadRequest("Username is required");
            if (payload.password == null) return TypedResults.BadRequest("Password is required");

            //load user from database
            var user = await userManager.FindByNameAsync(payload.username);
            if (user is null)
            {
                return TypedResults.BadRequest("Invalid username or password");
            }
            //check password matches
            bool isPasswordValid = await userManager.CheckPasswordAsync(user, payload.password);
            if (!isPasswordValid)
            {
                return TypedResults.BadRequest("Invalid username or password");
            }
            //create a token
            var token = tokenService.CreateToken(user);

            //return the response
            return TypedResults.Ok(new LoginResponseDTO(token, user.UserName, user.Role));
        }
    }
}
