using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataModels.DTOs;
using exercise.wwwapi.DataModels.Models;
using exercise.wwwapi.DataTransfer.Requests.User;
using exercise.wwwapi.DataTransfer.Responses;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// This somehow works. I converted the controller to an endpoint. I also had to use the repository to get the user because the userManager did not want to use FindEmailAsync properly.
/// </summary>
public static class UserEndpoint
{
    public static void ConfigureUserEndpoint(this WebApplication app)
    {
        var users = app.MapGroup("users");
        users.MapPost("/register", PostUser);
        users.MapPost("/login", LoginUser);
    }

    /// <summary>
    /// Registers a new user in the DB
    /// </summary>
    /// <param name="repository"></param>
    /// <param name="registerUser">A modelpost of Application user</param>
    /// <returns>A payload with DTO of ApplicationUser</returns>
    [ProducesResponseType(StatusCodes.Status201Created)]
    public static async Task<IResult> PostUser([FromServices] ApplicationUserRepository repository, RegistrationRequest registerUser)
    {
        if (registerUser.Email == null || registerUser.Password == null)
        {
            return Results.BadRequest("Email or password is missing");
        }

        var existingUser = await repository.SelectByEmail(registerUser.Email);
        if (existingUser != null)
        {
            return Results.BadRequest("Email is already registered.");
        }

        var newUser = new ApplicationUser
        {
            UserName = registerUser.Username,
            Email = registerUser.Email,
            Role = (Role)registerUser.Role // Convert integer to enum
        };

        var result = await repository.InsertUser(newUser, registerUser.Password);

        if (!result.Succeeded)
        {
            return Results.BadRequest("Failed to create user.");
        }

        var insertedUserDTO = EntityConverter.EntityMapper<ApplicationUserDTO>(result.Data, new List<string> { "Role" });
        insertedUserDTO.Role = newUser.Role.ToString();

        var payload = new Payload<ApplicationUserDTO>
        {
            Data = insertedUserDTO
        };

        return Results.Created($"/{result.Data.Id}", payload);
    }

    /// <summary>
    /// Logs in a user
    /// </summary>
    /// <param name="userManager"></param>
    /// <param name="authenticateUser"></param>
    /// <param name="tokenService"></param>
    /// <param name="repository"></param>
    /// <returns>A JWT token</returns>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> LoginUser([FromServices] UserManager<ApplicationUser> userManager, [FromBody] AuthRequest authenticateUser, [FromServices] TokenService tokenService, [FromServices] ApplicationUserRepository repository)
    {
        // For some reason, the userManager did not want to use FindEmailAsync, so I had to use the repository to get the user
        var managedUser = await repository.SelectByEmail(authenticateUser.Email);
        if (managedUser == null)
        {
            return Results.Unauthorized();
        }

        var isPasswordValid = await userManager.CheckPasswordAsync(managedUser, authenticateUser.Password);
        if (!isPasswordValid)
        {
            return Results.Unauthorized();
        }

        var accessToken = tokenService.CreateToken(managedUser);

        var authResponse = new AuthResponse
        {
            Username = managedUser.UserName,
            Email = managedUser.Email,
            Token = accessToken
        };

        return Results.Ok(authResponse);
    }

}

