using System.Runtime.CompilerServices;
using exercise.minimalapi.DTOs;
using exercise.minimalapi.Helpers;
using exercise.minimalapi.Models;
using exercise.minimalapi.Repositories.AuthRepo;
using exercise.minimalapi.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exercise.minimalapi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/auth");
            authGroup.MapPost("/register", Register);
            authGroup.MapPost("/login", Login);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Register(RegisterDto registerPayload, UserManager<ApplicationUser> userManager)
        {
            if (registerPayload.Email == null) { return TypedResults.BadRequest("Email is required"); }
            if (registerPayload.Password == null) { return TypedResults.BadRequest("Password is required"); }

            var result = await userManager.CreateAsync(
                new ApplicationUser
                {
                    UserName = registerPayload.Email,
                    Email = registerPayload.Email,
                    Role = Enums.UserRole.User
                },
            registerPayload.Password!
            );

            if(result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new RegisterResponseDto(registerPayload.Email, Enums.UserRole.User));
            }
            return TypedResults.BadRequest(result.Errors);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> Login(
            LoginDto loginPayload, 
            UserManager<ApplicationUser> userManager, 
            TokenService tokenService,
            IAuthRepo repository)
        {
            if (loginPayload.Email == null) { return TypedResults.BadRequest("Email is required"); }
            if (loginPayload.Password == null) { return TypedResults.BadRequest("Password is required"); }

            var user = await userManager.FindByEmailAsync(loginPayload.Email);
            if (user == null) { return TypedResults.BadRequest("Invalid email or password"); }

            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginPayload.Password);
            if (!isPasswordValid) { return TypedResults.BadRequest("Invalid email or password"); }

            var userInDb = await repository.GetUserAsync(loginPayload.Email);

            if (userInDb == null) { return Results.Unauthorized(); }

            var accessToken = tokenService.CreateToken(userInDb);
            return TypedResults.Ok(new AuthResponseDto(userInDb.Id, accessToken, userInDb.Email, userInDb.Role.ToString()));
        }
    }
}
