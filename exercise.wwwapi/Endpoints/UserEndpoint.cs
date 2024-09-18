
using exercise.wwwapi.DataTransfer.Request;
using exercise.wwwapi.DataTransfer.Response;
using exercise.wwwapi.Enum;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Net.Mime.MediaTypeNames;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoint
    {
        public static void ConfigureUserEndpoint(this WebApplication app)
        {
            //app.MapPost("/api/users/register", Register);
            //app.MapPost("/api/users/login", LoginRequest);
           // app.MapPut("/api/users/update/{id}", EditUser);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> LoginRequest(
            LoginUser loginrequest,
            UserManager<ApplicationUser> userManager, 
            TokenService tokenService,
            IRepository<BlogPost> repository)
        {
            //check body-request - usercredentials
            if (loginrequest.Email == null) return
    TypedResults.BadRequest("Email is required.");
            if (loginrequest.Password == null) return
            TypedResults.BadRequest("Password is required.");

            // load the user from database
            var user = await
            userManager.FindByEmailAsync(loginrequest.Email!);
            if (user == null)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }

            // check the password matches
            var isPasswordValid = await
            userManager.CheckPasswordAsync(user, loginrequest.Password);
            if (!isPasswordValid)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }

            var token = tokenService.CreateToken(user);
            // return the response
            return TypedResults.Ok(new AuthResponse()
            {
                Username = user.UserName,
                Token = token,
                Email = user.Email
            });

        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> Register(RegisterUser request, UserManager<ApplicationUser> userManager)
        {
            if (request.Email == null){ return TypedResults.BadRequest("Email is required."); }
            if (request.Password == null) { return TypedResults.BadRequest("Password is required."); }
            var result = await userManager.CreateAsync(
            new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                Role = request.Role
            },
            request.Password!
            );
            Console.Write(result);
            if (result.Succeeded)
            {
                return TypedResults.Created($"/api/", new RegisterResponse(email: request.Email, userRole:request.Role));
            }
            return Results.BadRequest(result.Errors);
        }
        /*

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async static Task<IResult> EditUser(PatchUser updaterequest,
            UserManager<ApplicationUser> userManager,
            IRepository<ApplicationUser> repository, string id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find User with Username:{id}");
            }
            userManager.UpdateAsync(entity);
            entity.UserName = !string.IsNullOrEmpty(updaterequest.UserName) ? updaterequest.UserName : entity.UserName;
            entity.Role = !string.IsNullOrEmpty(updaterequest.Role.ToString()) ? updaterequest.Role : entity.Role;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { username = result.UserName, role = result.Role }) : TypedResults.BadRequest("Couldn't save to the database?!");
        
        }*/

    }
}
