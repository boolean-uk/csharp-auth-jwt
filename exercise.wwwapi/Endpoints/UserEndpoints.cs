
using api_cinema_challenge.DTO;
using api_cinema_challenge.Repository;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Models;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoints
    {
        public static void ConfigureUserEndpoints(this WebApplication app)
        {
            var usergroup = app.MapGroup("user");
            usergroup.MapPost("/", CreateUser);
        }

        private static async Task<IResult> CreateUser(HttpContext context, IRepository<User> repo, Create_User dto)
        {
            User? user = await Create_User.create(repo, dto);
            return user == null ? TypedResults.BadRequest() : TypedResults.Ok(Get_User.toPayload(user));
            
        }
    }
}
