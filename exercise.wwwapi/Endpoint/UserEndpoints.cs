using System.Data;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoint
{
    public static class UserEndpoints
    {
        public static void ConfigureUserEndpoints(this WebApplication app)
        {
            var userEndpoints = app.MapGroup("/api/user");
            userEndpoints.MapPost("/{userId}/follows/{targetUserId}", FollowUser).RequireAuthorization("AdminOrAuthor");
            userEndpoints.MapPost("/{userId}/unfollows/{targetUserId}", UnfollowUser).RequireAuthorization("AdminOrAuthor");
            userEndpoints.MapGet("/api/viewall/{userId}", GetPostsByFollowedUsers).RequireAuthorization("AdminOrAuthor");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> FollowUser(int userId, int targetUserId, [FromServices] IUserRepository userRepository)
        {
            await userRepository.FollowUser(userId, targetUserId);
            return TypedResults.Ok($"User {userId} followed {targetUserId}");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> UnfollowUser(int userId, int targetUserId, [FromServices] IUserRepository userRepository)
        {
            await userRepository.UnfollowUser(userId, targetUserId);
            return TypedResults.Ok($"User {userId} unfollowed {targetUserId}");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetPostsByFollowedUsers(int userId, [FromServices] IBlogPostRepository repository, [FromServices] IUserRepository userRepository)
        {
            var followedUsers = await userRepository.GetFollowedUsers(userId);

            if (followedUsers == null || !followedUsers.Any()) return TypedResults.NotFound("No followed users found.");

            var posts = await repository.GetPostsByUsers(followedUsers.Select(u => u.Id.ToString()).ToList()); // Convert User objects to their IDs for the post repository

            return TypedResults.Ok(posts);
        }
    }
}
