using exercise.wwwapi.DTOs;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoint
    {
        private static string _path = AppContext.BaseDirectory;
        public static void UserEndpointConfiguration(this WebApplication app)
        {
            var users = app.MapGroup("user");
            users.MapGet("/follow/{id}", FollowUser);
            //users.MapPost("/unfollow/{id}", UnfollowUser);
            //users.MapPut("/viewall/{id}", ViewAllFollowingUserPosts);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> FollowUser(IRepository<User> repository, int id, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await repository.Get(x => x.Id == userId);
            if (!result.FollowingUsersIds.Contains(id))
            {
                result.FollowingUsersIds.Add(id);
            }
            
            result = await repository.Update(result);

            User updatedUser = await repository.Get(x => x.Id == result.Id);

            var resultDTO = new UserResponseDTO(updatedUser);

            var payload = new Payload<UserResponseDTO>() { data = resultDTO };
            return TypedResults.Created(_path, payload);
        }
    }
}
