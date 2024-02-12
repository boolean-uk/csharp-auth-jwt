using System;
using System.Net.Http;
using System.Threading.Tasks;
using auth.DTO;
using auth.Model;
using auth.Model.Payload;
using auth.Repository;
using auth.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace auth.Endpoints
{
    public static class UsersEndpoints
    {
        public static void ConfigureUsersEndpoints(this WebApplication app)
        {
            var accounts = app.MapGroup("/users");
            accounts.MapPost("/register", Register);
            accounts.MapPost("/registerAdmin", RegisterAdmin);
            accounts.MapPost("/login", Login);
        }

        private static async Task<IResult> Login(UserManager<User> userManager, TokenService tokenService, IUserRepository repository, CreateUserPayload payload)
        {
            if (payload.Email == null)
                return TypedResults.BadRequest("Email is required.");
            if (payload.Password == null)
                return TypedResults.BadRequest("Password is required.");

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

            var userInDb = repository.GetUser(payload.Email);

            if (userInDb is null)
            {
                return Results.Unauthorized();
            }

            var accessToken = tokenService.CreateToken(userInDb);
            return TypedResults.Ok(new AuthResponseDto(accessToken, userInDb.Email, userInDb.Role, userInDb.Id));
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Register(UserManager<User> userManager, CreateUserPayload payload)
        {
            if (payload.Email == null)
                return TypedResults.BadRequest("Email is required.");
            if (payload.Password == null)
                return TypedResults.BadRequest("Password is required.");

            var result = await userManager.CreateAsync(
                new User { UserName = payload.Email, Email = payload.Email, Role = UserRole.User },
                payload.Password!
            );

            if (result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new { email = payload.Email, role = UserRole.User });
            }
            return Results.BadRequest(result.Errors);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> RegisterAdmin(UserManager<User> userManager, CreateUserPayload payload)
        {
            if (payload.Email == null)
                return TypedResults.BadRequest("Email is required.");
            if (payload.Password == null)
                return TypedResults.BadRequest("Password is required.");

            var result = await userManager.CreateAsync(
                new User { UserName = payload.Email, Email = payload.Email, Role = UserRole.Admin },
                payload.Password!
            );

            if (result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new { email = payload.Email, role = UserRole.Admin });
            }
            return Results.BadRequest(result.Errors);
        }
    }
}
