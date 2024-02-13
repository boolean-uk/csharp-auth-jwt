using exercise.minimalapi.DTOs;
using exercise.minimalapi.Enums;
using exercise.minimalapi.Models;
using exercise.minimalapi.Repositories.AuthRepo;
using exercise.minimalapi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.minimalapi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("/auth");
            authGroup.MapPost("/register", Register);
            authGroup.MapPost("/login", Login);
            authGroup.MapDelete("/delete/{email}", Delete);
            authGroup.MapPatch("/Update/{email}", UpdateUserRole);
            authGroup.MapGet("/users", GetAllUsers);
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

            if (result.Succeeded)
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

        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(UserManager<ApplicationUser> userManager, IAuthRepo repository, string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email)) { return TypedResults.BadRequest("Email is required"); }

                var user = await repository.GetUserAsync(email);
                if (user == null) { return TypedResults.NotFound("User not found"); }

                var deleteResult = await userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    return TypedResults.BadRequest("Failed to delete user");
                }

                return TypedResults.Ok(
                    new DeletedUserDto(
                    user.Id,
                    user.NormalizedUserName,
                    user.NormalizedEmail,
                    user.Role.ToString()
                    )
                );
            }
            catch (Exception e)
            {

                return TypedResults.BadRequest(e.Message);
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateUserRole(IAuthRepo repository, UserRoleUpdateDto userUpdates)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userUpdates.email)) { throw new ArgumentException("Email is required"); }
                if (!Enum.IsDefined(userUpdates.Role)) { throw new ArgumentException("Not a valid role"); }

                var user = await repository.ChangeUserRoleAsync(userUpdates.email, userUpdates.Role);
                return TypedResults.Ok(new RegisterResponseDto(user.Email, user.Role));
            }
            catch (ArgumentNullException e)
            {
                return TypedResults.NotFound(e.Message);
            }
            catch (Exception e)
            {
                return TypedResults.BadRequest(e.Message);
            }
        }

        [Authorize(Roles = nameof(UserRole.Admin))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAllUsers(IAuthRepo repository)
        {
            var users = await repository.GetAllUsersAsync();
            var usersResult = new List<RegisterResponseDto>();
            foreach (var user in users)
            {
                usersResult.Add(new RegisterResponseDto(user.Email, user.Role));
            }
            return TypedResults.Ok(usersResult);
        }
    }
}