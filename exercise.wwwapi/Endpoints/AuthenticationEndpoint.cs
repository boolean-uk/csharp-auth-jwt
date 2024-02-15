using AutoMapper;
using exercise.Application;
using exercise.Data.Models;
using exercise.Infrastructure;
using exercise.wwwapi.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthenticationEndpoint
    {
        public static void ConfigureAuthenticationEndpoint(this WebApplication app)
        {
            var group = app.MapGroup("/auth");
            group.MapPost("/register", Register);
            group.MapPost("/login", Login);
        }

        public static async Task<IResult> Register(
            [FromServices] UserManager<User> repository,
            [FromBody] AddUserDTO addUserDTO,
            [FromServices] IMapper mapper,
            [FromServices] TokenService authService
        )
        {
            ServiceResponse<string> response = new();
            try
            {
                var result = await repository.CreateAsync(
                    new User() { 
                        UserName = addUserDTO.UserName, 
                        Email = addUserDTO.Email, 
                        PhoneNumber = addUserDTO.PhoneNumber, 
                        Role = addUserDTO.Role 
                    }, 
                    addUserDTO.Password);
                if (result.Succeeded)
                {
                    User user = await repository.FindByNameAsync(addUserDTO.UserName);
                    string token = authService.CreateToken(user);
                    response.Data = token;
                    return TypedResults.Created(nameof(Register), response);
                }
                response.Success = false;
                response.Data = result.Errors.ToList()[0].Description;
                return TypedResults.BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return TypedResults.BadRequest(response);
            }
        }

        public static async Task<IResult> Login(
            [FromServices] UserManager<User> repository,
            [FromServices] IMapper mapper,
            [FromServices] TokenService authService,
            [FromBody] LoginDTO loginDTO)
        {
            ServiceResponse<string> response = new();
            try
            {
                User user = await repository.FindByNameAsync(loginDTO.UserName)
                    ?? throw new Exception($"No user with username: {loginDTO.UserName}");
                bool isValidPassword = await repository.CheckPasswordAsync(user, loginDTO.Password);
                if(!isValidPassword)
                {
                    response.Success = false;
                    response.Message = "Incorrect username or password.";
                    return TypedResults.BadRequest(response);
                }
                string token = authService.CreateToken(user);
                response.Data = token;
                return TypedResults.Ok(response);
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return TypedResults.BadRequest(response);
            }

        }
    }
}
