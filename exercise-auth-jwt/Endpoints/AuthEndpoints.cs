using exercise_auth_jwt.DataModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using exercise_auth_jwt.DTO;
using System.Runtime.CompilerServices;
using exercise_auth_jwt.Enum;
using exercise_auth_jwt.Services;
using exercise_auth_jwt.Repository;

namespace exercise_auth_jwt.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var taskGroup = app.MapGroup("auth");
            taskGroup.MapPost("/register", Register);
            taskGroup.MapPost("/login", Login);

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async static Task<IResult> Register(RegisterDto registerPayload, UserManager<ApplicationUser> userManager)
        {
            if(registerPayload.Email == null)
            {
                return TypedResults.BadRequest("Email is required.");
            }
            if(registerPayload.Password == null)
            {
                return TypedResults.BadRequest("Password is required.");
            }
            var result = await userManager.CreateAsync(
                new ApplicationUser
                {
                    UserName = registerPayload.Email,
                    Email = registerPayload.Email,
                    Role = UserRole.User
                },
                registerPayload.Password!
                );

            if(result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new RegisterResponseDto(registerPayload.Email, UserRole.User));
            }
            return Results.BadRequest(result.Errors);
        }

        public async static Task<IResult> Login(LoginDto loginPayload, 
            UserManager<ApplicationUser> userManager,
            TokenService tokenService,
            IRepository repository)
        {
            if(loginPayload.Email == null)
            {
                return TypedResults.BadRequest("Email is required.");
            }
            if(loginPayload.Password == null) 
            {
                return TypedResults.BadRequest("Password is required.");
            }

            var user = await userManager.FindByEmailAsync(loginPayload.Email!);
            if(user == null)
            {
                return TypedResults.BadRequest("Invalid Email or Password.");
            }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginPayload.Password);
            if(!isPasswordValid) 
            {
                return TypedResults.BadRequest("Invalid Email or Password.");
            }

            var token = tokenService.CreateToken(user);

            return TypedResults.Ok(new AuthResponseDto(token, user.Email, user.Role));
        }
    }
}
