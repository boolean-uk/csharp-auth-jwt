using exercise.wwwapi.Data;
using exercise.wwwapi.Data.DTO;
using exercise.wwwapi.Data.Enums;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoint
    {
        public static void ConfigureUserEnpoint(this WebApplication app)
        {
            var userGroup = app.MapGroup("users");
            userGroup.MapPost("/register", CreateUser);
            userGroup.MapPost("/login", LoginUser);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> CreateUser(CreateUserDTO cDTO, IRepository repository)
        {
            ResponseObject<string> createdObject = await repository.CreateUser(cDTO);
            if (createdObject.Status == ResponseStatus.Failure) return TypedResults.BadRequest(createdObject);
            return TypedResults.Created(" ", createdObject);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> LoginUser(LoginUserDTO lDTO, IRepository repository)
        {
            ResponseObject<string> loginObject = await repository.LoginUser(lDTO);
            if (loginObject.Status == ResponseStatus.Failure) { return TypedResults.BadRequest(loginObject); }
            return TypedResults.Ok(loginObject);
        }
    }
}
