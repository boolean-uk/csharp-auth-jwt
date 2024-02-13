using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Authentication.Model;
using Authentication.DTO;
using Authentication.Services;
using Authentication.Enums;
using Authentication.Repository;

namespace Authentication.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var taskGroup = app.MapGroup("auth");
            taskGroup.MapPost("/register", Register);
            taskGroup.MapPost("/login", Login);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Register(RegisterDto registerPayload, UserManager<ApplicationUser> userManager)
        {
            //Parameter checks
            if (registerPayload.UserName == null) return TypedResults.BadRequest("Username is required.");
            if (registerPayload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (registerPayload.Password == null) return TypedResults.BadRequest("Password is required.");

            //Creates the user
            var result = await userManager.CreateAsync(new ApplicationUser { UserName = registerPayload.UserName, Email = registerPayload.Email, Role = UserRole.User },registerPayload.Password!);
            if (result.Succeeded)
                return TypedResults.Created($"/auth/", new { username = registerPayload.UserName, email = registerPayload.Email, role = UserRole.User });

            return Results.BadRequest(result.Errors);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Login(LoginDto loginPayload, UserManager<ApplicationUser> userManager, TokenService tokenService, IRepository repository)
        {
            //Parameter checks
            if (loginPayload.UserName == null) return TypedResults.BadRequest("Username is required.");
            if (loginPayload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (loginPayload.Password == null) return TypedResults.BadRequest("Password is required.");

            //Trying to find the user
            var user = await userManager.FindByNameAsync(loginPayload.UserName);
            if (user == null)
                return TypedResults.BadRequest("Invalid username, email or password.");

            var email = await userManager.FindByEmailAsync(loginPayload.Email!);
            if (email == null)
                return TypedResults.BadRequest("Invalid username, email or password.");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginPayload.Password);
            if (!isPasswordValid)
                return TypedResults.BadRequest("Invalid username, email or password.");

            //Creates token and outputs it
            var accessToken = tokenService.CreateToken(user);
            return TypedResults.Ok(new AuthResponseDto(accessToken, user.UserName, user.Email, user.Role));
        }
    }
}