using exercise.wwwapi.DTO;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthEndpoints
    { 
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("auth");
            authGroup.MapPost("/register", Register);
            authGroup.MapPost("/login", Login);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async static Task<IResult> Login(LoginDTO payload, UserManager<ApplicationUser> userManager, IRepository repository, TokenService tokenService)
        {
            //Check payload contains name, email and password
            if (payload.Name == null)
            {
                return TypedResults.BadRequest("You must enter a name");
            }
            if (payload.Email == null)
            {
                return TypedResults.BadRequest("You must enter an email address");
            }
            if (payload.Password == null)
            {
                return TypedResults.BadRequest("You must enter a password");
            }
            //Load user
            var user = await userManager.FindByEmailAsync(payload.Email);
            if(user == null)
            {
                return TypedResults.BadRequest("Invalid email or password");
            }
            //Check password and email is a match (Don't say what fails pass/email)
            var validPass = await userManager.CheckPasswordAsync(user, payload.Password);
            if (!validPass)
            {
                return TypedResults.BadRequest("Invalid email or password");
            }
            var token = tokenService.CreateToken(user);
            return TypedResults.Ok(new AuthResponseDTO(payload.Name, token, user.Email, user.Role));
        }

        public async static Task<IResult> Register(RegisterDTO payload, UserManager<ApplicationUser> userManager)
        {
            //Check payload contains email and password
            if(payload.Email == null)
            {
                return TypedResults.BadRequest("You must enter an email address");
            }
            if(payload.Password == null)
            {
                return TypedResults.BadRequest("You must enter a password");
            }
            //Save to DB
            var user = await userManager.CreateAsync(
                new ApplicationUser
                {
                    Name = payload.Name,
                    UserName = payload.Email,
                    Email = payload.Email,
                    Role = UserRole.User
                },
                payload.Password!
            );
            //Check user was created
            if (user.Succeeded)
            {
                return TypedResults.Created($"/auth/", new RegisterResponseDTO(payload.Name, payload.Email, UserRole.User));
            }
            return TypedResults.BadRequest(user.Errors);
        }
    }
}
