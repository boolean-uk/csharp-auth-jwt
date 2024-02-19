using exercise.wwwapi.DataModels.Models;
using exercise.wwwapi.DataTransfer.Requests.User;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;

public static class UserEndpoint
{
    public static void ConfigureUserEndpoint(this WebApplication app)
    {
        var users = app.MapGroup("users");
        //users.MapPost("/register", PostUser);
        //users.MapGet("/login", LoginUser);
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    public static async Task<IResult> PostUser(IRepository<ApplicationUser> repository, RegistrationRequest registerUser)
    {
        throw new NotImplementedException();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public static async Task<IResult> LoginUser(IRepository<ApplicationUser> repository, AuthRequest authenticateUser)
    {
        throw new NotImplementedException();
    }
}

