using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static exercise.wwwapi.DTO.DTO;
using static exercise.wwwapi.DTO.Payload;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthEndpoint
    {
        public static void ConfigureAuthEndpoint(this WebApplication app)
        {
            var taskGroup = app.MapGroup("auth");
            taskGroup.MapPost("/register", Register);
            taskGroup.MapPost("/registeradmin", RegisterAdmin);
            taskGroup.MapPost("/login", Login);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async static Task<IResult> Register(CreateApplicationUserPayload Payload, UserManager<ApplicationUser> userManager)
        {
            if (Payload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (Payload.Password == null) return TypedResults.BadRequest("Password is required.");
            var result = await userManager.CreateAsync(
            new ApplicationUser
            {
                UserName = Payload.Name,
                Email = Payload.Email,
                Role = UserRole.User
            },
            Payload.Password!
            );
            if (result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new CreateUserDTO(Payload.Email, UserRole.User));
            }
            return Results.BadRequest(result.Errors);
        }
        public async static Task<IResult> RegisterAdmin(CreateApplicationUserPayload Payload, UserManager<ApplicationUser> userManager)
        {
            if (Payload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (Payload.Password == null) return TypedResults.BadRequest("Password is required.");
            var result = await userManager.CreateAsync(
            new ApplicationUser
            {
                UserName = Payload.Name,
                Email = Payload.Email,
                Role = UserRole.Admin
            },
            Payload.Password!
            );
            if (result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new CreateUserDTO(Payload.Email, UserRole.Admin));
            }
            return Results.BadRequest(result.Errors);
        }
        public async static Task<IResult> Login(LoginPayload Payload, UserManager<ApplicationUser> userManager, TokenService tokenService, IRepository repository)
        {
            if (Payload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (Payload.Password == null) return TypedResults.BadRequest("Password is required.");

            var user = await
            userManager.FindByEmailAsync(Payload.Email!);
            if (user == null)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }
            
            var isPasswordValid = await
            userManager.CheckPasswordAsync(user, Payload.Password);
            if (!isPasswordValid)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }
            
            var token = tokenService.CreateToken(user);
            
            return TypedResults.Ok(new AuthResponseDTO(token, user.Email, user.Role));
        }
    }
}
