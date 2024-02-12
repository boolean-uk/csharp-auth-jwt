using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using exercise.wwwapi.Models;
using exercise.wwwapi.DTO;
//using exercise.wwwapi.Services;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
namespace exercise.wwwapi.Endpoints
{
    public static class AuthEnpoints
    {
        public static void ConfigureAuthApi(this WebApplication app)
        {
            var taskGroup = app.MapGroup("auth");
            taskGroup.MapPost("/register", Register);
            taskGroup.MapPost("/login", Login);
        }

        //using usermanager to interact with the user table. UserManager is from identity framework
        private static async Task<IResult> Register(UserManager<Users> userManager, RegisterDto payload)
        {
            //ensure we recieve right payload of data
            if (payload.Email == null) return TypedResults.BadRequest("Email is required");
            if (payload.Password == null) return TypedResults.BadRequest("Password is required");


            // create a new user through usermanager
            // using the rules from program.cs for identity
            var result = await userManager.CreateAsync(
                new Users { UserName = payload.Email, Email = payload.Email, Role = UserRole.User },
              payload.Password! // ! means hashed
            );

            if (result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new RegisterResponseDto(payload.Email, UserRole.User));
            }
            return Results.BadRequest(result.Errors);
        }

        private static async Task<IResult> Login(TokenService tokenService, IRepository repository, LoginDto payload, UserManager<Users> userManager)
        {
            //ensure we recieve right payload of data

            if (payload.Email == null) return TypedResults.BadRequest("Email is required");
            if (payload.Password == null) return TypedResults.BadRequest("Password is required");

            //load user from database
            Users? user = await userManager.FindByEmailAsync(payload.Email);
            if (user == null)
            {
                return TypedResults.BadRequest("Email or Password incorrect");
            }
            //check password matches
            bool isPasswordValid = await userManager.CheckPasswordAsync(user, payload.Password);
            if (!isPasswordValid)
            {
                return TypedResults.BadRequest("Email or Password incorrect");
            }
            //create a token for performing other actions as a logged in user
            var token = tokenService.CreateToken(user);
            //return the resposne 
            return TypedResults.Ok(new AuthResponseDto(token, user.Email, user.Role));
        }
    }
}

