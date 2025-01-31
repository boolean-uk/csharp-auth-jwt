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

    private static async Task<IResult> LoginUser(IRepository<User> repository, IMapper mapper,
        [FromBody] LoginPost body)
    {
        var user = await repository.Get(u => u.Username == body.Username);
        if (user == null || !user.ValidatePassword(body.Password))
            return TypedResults.NotFound(new BaseResponse<object?>(Consts.ErrorStatus, null));

        var token = Jwt.CreateToken(user);

        var response = new BaseResponse<string>(
            Consts.SuccessStatus,
            token
        );

        return TypedResults.Ok(response);
    }

    private static async Task<IResult> RegisterUser(IRepository<User> repository, IMapper mapper,
        [FromBody] UserPost body)
    {
        //var user = mapper.Map<User>(body);

        var user = new User
        {
            DisplayName = body.DisplayName,
            Username = body.Username,
            Email = body.Email,
            Password = body.Password
        };
        await repository.Add(user);
        var response = new BaseResponse<UserResponse>(
            Consts.SuccessStatus,
            mapper.Map<UserResponse>(user)
        );

        return TypedResults.Created($"/users/{user.Id}", response);
    }
}