using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints;

public static class UserEndpoints
{
    public static void ConfigureUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("users");
        
        group.MapPost("register", RegisterUser);
    }

    private static async Task<IResult> RegisterUser(IRepository<User> repository, IMapper mapper, [FromBody] UserPost body)
    {
        //var user = mapper.Map<User>(body);
        
        User user = new User
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