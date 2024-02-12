using exercise.wwwapi.auth.Models;
using exercise.wwwapi.auth.Enums;
using Microsoft.AspNetCore.Identity;
using static exercise.wwwapi.auth.Payloads.AuthPayloads;
using exercise.wwwapi.auth.Payloads;
using exercise.wwwapi.auth.Services;

namespace exercise.wwwapi.auth.Endpoints
{
    public static class AuthApi
    {
        public static void ConfigureAuthApi(this WebApplication app)
        {
            var authGroup = app.MapGroup("authentication");
            authGroup.MapPost("/register", Register);
            authGroup.MapPost("/login", Login);
        }



        public static async Task<IResult> Register(UserManager<ApplicationUser> userManager, RegisterPayload payload)
        {
            if (payload.Email == null) return TypedResults.BadRequest("Email is required!");
            if (payload.Password == null || payload.Email == "") return TypedResults.BadRequest("Password is required!");

            
            var result = await userManager.CreateAsync(
                new ApplicationUser
                {
                    UserMadeUserName = payload.Username,
                    UserName = payload.Username,
                    Email = payload.Email,

                    Role = UserRole.User
                },
                payload.Password!
            );

            if (result.Succeeded)
            {
                return TypedResults.Created($"/authentication/register", new RegisterResPayload(payload.Email, UserRole.User));
            }
            else return TypedResults.BadRequest(result.Errors);
            
        }

        public static async Task<IResult> Login(LoginPayload payload, UserManager<ApplicationUser> userManager, TokenService tokenService)
        {
            if (payload.Email == null || payload.Email == "") return TypedResults.BadRequest("Email is required!");
            if (payload.Password == null || payload.Password == "") return TypedResults.BadRequest("Password is required!");

            var user = await userManager.FindByEmailAsync(payload.Email);
            if (user == null) return TypedResults.BadRequest("Invalid Email and/or Password");
            
            var isPassordValid = await userManager.CheckPasswordAsync(user, payload.Password);
            if (!isPassordValid) return TypedResults.BadRequest("Invalid Email and/or Password");

            var token = tokenService.CreateToken(user);

            return TypedResults.Ok(new LoginResPayload(token, user.Email, user.Role));
            
            
            
            throw new NotImplementedException();
        }
    }
}
