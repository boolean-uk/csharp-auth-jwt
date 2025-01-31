using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints;

public static class UserEndpoints
{
    public static void ConfigureUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users");

        group.MapPost("login", LoginUser);
        group.MapPost("register", RegisterUser);
    }

    [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    private static async Task<IResult> LoginUser(IRepository<User> repository, IMapper mapper,
        [FromBody] LoginPost body)
    {
        var user = await repository.Get(u => u.Username == body.Username);
        if (user == null || !user.ValidatePassword(body.Password))
            return TypedResults.Unauthorized();

        var token = Jwt.CreateToken(user);

        var response = new BaseResponse<string>(
            Consts.SuccessStatus,
            token
        );

        return TypedResults.Ok(response);
    }

    [ProducesResponseType(typeof(BaseResponse<UserResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    private static async Task<IResult> RegisterUser(IRepository<User> repository, IMapper mapper,
        [FromBody] UserPost body)
    {
        var user = mapper.Map<User>(body);
        
        // Check if username or email is taken. No duplicates allowed
        if (await repository.Get(u => u.Username == user.Username) != null)
            return TypedResults.Conflict("Username is already registered");
        if (await repository.Get(u => u.Email == user.Email) != null)
            return TypedResults.Conflict("Email is already registered");
        
        await repository.Add(user);
        var response = new BaseResponse<UserResponse>(
            Consts.SuccessStatus,
            mapper.Map<UserResponse>(user)
        );

        return TypedResults.Created($"/users/{user.Id}", response);
    }
}