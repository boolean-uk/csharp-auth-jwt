using Microsoft.AspNetCore.Mvc;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Identity;
using exercise.wwwapi.Enums;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Services;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var postGroup = app.MapGroup("auth");
            postGroup.MapPost("/register", Register);
            postGroup.MapPost("/login", Login);
        }

        public async static Task<IResult> Register(RegisterDto registerPayload, 
            UserManager<ApplicationUser> userManager)//using the identity framework, UserManager class that accpets an ApplicationUser
        {
            if (registerPayload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (registerPayload.Password == null) return TypedResults.BadRequest("Password is required.");

            var result = await userManager.CreateAsync(
                new ApplicationUser //using the userManager method create to create a new Application user
                {
                    UserName = registerPayload.Email, //quirk of the identity framework, it expects to provide a username, cannot be disregarded
                    Email = registerPayload.Email,
                    Role = UserRole.User //assigneing the user role by default
                },
                registerPayload.Password! //specified rquiremnets for password in program.cs, those checks will be performed on the password
                                          //and then the password will be stored as the hashed version, which is safe
            );
            if (result.Succeeded) // if the creaated succeed return the email and user role from the DTOs
            {
                return TypedResults.Created($"/auth/", new RegisterResponseDto(registerPayload.Email, UserRole.User));
            }
            return Results.BadRequest(result.Errors); //if created not suceeded return error
        }


        public async static Task<IResult> Login(LoginDto loginPayload, UserManager<ApplicationUser> userManager, 
            TokenService tokenService, IRepository repository)//need the repository to perform extra check on the context, and tokenservice through dependency injection
        {
            // check payload
            if (loginPayload.Email == null) return TypedResults.BadRequest("Email is required.");
            if (loginPayload.Password == null) return TypedResults.BadRequest("Password is required.");
            
            // load the user from database
            var user = await userManager.FindByEmailAsync(loginPayload.Email!); //ask to retrieve a suer by email, build in method of the userManager
            if (user == null)
            {
                return TypedResults.BadRequest("Invalid email or password."); //don't wan't hackers to know if the problem is the mail or password
                                                                              //could return not found, but better to return bad request to make hackers guess more
            }

            // check the password matches
            var isPasswordValid = await userManager.CheckPasswordAsync(user, loginPayload.Password); //ask the userManager, given a specifc user and a password
                                                                                                     //tell me if the password matches the hash stored in the database,
                                                                                                     // will get true or false
            if (!isPasswordValid)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }

            // create a token
            var token = tokenService.CreateToken(user);

            // return the response
            return TypedResults.Ok(new AuthResponseDto(token, user.Email, user.Role));
        }



    }
}
