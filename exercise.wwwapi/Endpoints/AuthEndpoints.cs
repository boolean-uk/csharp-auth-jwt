using exercise.wwwapi.Enums;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Service;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Endpoints
{

    public record RegisterDTO(string Email, string Password);
    public record RegisterResponseDto(string Email, UserRole
    Role);
    public record LoginDto(string Email, string Password);
    public record AuthResponseDto(string Token, string Email, UserRole Role);

    public static class AuthEndpoints
    {


        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("auth");
            authGroup.MapPost("/Register", Register);
            authGroup.MapPost("/Login", Login);
        }

        public async static Task<IResult> Register(RegisterDTO payload, UserManager<ApplicationUser> userManager)
        {
            if (payload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (payload.Password == null) return TypedResults.BadRequest("Password is required.");

            var result = await userManager.CreateAsync(
                new ApplicationUser
                {
                    UserName = payload.Email,
                    Email = payload.Email,
                    Role = UserRole.User
                }, payload.Password!
                );

            if (result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new RegisterResponseDto(payload.Email, UserRole.User));
            }
            return TypedResults.BadRequest(result.Errors);
        }
        public async static Task<IResult> Login(LoginDto payload, UserManager<ApplicationUser> userManager, TokenService tokenService, IRepository Repository)
        {

            if (payload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (payload.Password == null) return TypedResults.BadRequest("Password is required.");

            var user = await userManager.FindByEmailAsync(payload.Email!);
            if (user == null)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }
            var isPasswordValid = await userManager.CheckPasswordAsync(user, payload.Password);

            if (!isPasswordValid)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }

            // create a token
            var token = tokenService.CreateToken(user);
            // return the response
            return TypedResults.Ok(new AuthResponseDto(token,
            user.Email, user.Role));

            throw new NotImplementedException();
        }
    }
}