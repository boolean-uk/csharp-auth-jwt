using exercise.wwwapi.auth.Models;
using exercise.wwwapi.auth.Enums;
using Microsoft.AspNetCore.Identity;
using static exercise.wwwapi.auth.Payloads.AuthPayloads;
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


        /// <summary>
        /// Register a user that is not logged in or authenticated
        /// </summary>
        /// <param name="userManager"></param>  is the class from Identity that is used to access the users table that was generated
        /// <param name="payload"></param> is the data we want the user to provide
        /// <returns></returns> 201 if created, 400 if payload is missing or bad
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

        /// <summary>
        /// Makes so a user that is registered can authenticate itself 
        /// </summary>
        /// <param name="payload"></param> the data we want the user to provide
        /// <param name="userManager"></param> is the class from Identity that is used to access the users table that was generated
        /// <param name="tokenService"></param> is the TokenServer class that creates a JWT token easily
        /// <returns></returns> 200 if the payload is ok and the user is in the database, 400
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
        }
    }
}
