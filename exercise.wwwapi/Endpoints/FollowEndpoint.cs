using exercise.wwwapi.Configuration;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.DataViews;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    // The follow endpoint requires an user to be logged in,
    // it will use the token of the user to get its id from the db
    public static class FollowEndpoint
    {
        private static string _path = AppContext.BaseDirectory;
        public static void ConfigureFollowEndpoint(this WebApplication app)
        {
            app.MapPost("follows/{id}", Follow);
            app.MapDelete("unfollows{id}", Unfollow);
            app.MapGet("viewallfollowing", GetAllFollowingPosts);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllFollowingPosts(IRepository<Follow> service, ClaimsPrincipal user)
        {
            // get the id of the current user from the claims, that is stored in the token
            var userid = int.Parse(user.FindFirstValue(ClaimTypes.Sid)!);

            // Get all follows that the logged in user has
            var results = await service.GetAll(["User", "OtherUser"], u => u.UserId == userid);

            // Loop through all the users that the logged in user follows, and get all their blog posts
            var resultDTOs = new List<BlogPostDTO>();
            foreach (var result in results)
            {
                foreach (var post in result.OtherUser.BlogPosts)
                {
                    resultDTOs.Add(new BlogPostDTO(post));
                }
            }

            var payload = new Payload<IEnumerable<BlogPostDTO>>() { Data = resultDTOs };
            return TypedResults.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Follow(IRepository<Follow> service, int id, ClaimsPrincipal user)
        {
            // get the id of the current user from the claims, that is stored in the token
            var userid = int.Parse(user.FindFirstValue(ClaimTypes.Sid)!);

            var follow = new Follow() { UserId = userid, OtherUserId = id };
            var result = await service.Create(["User", "OtherUser"], follow);
            var resultDTO = new FollowDTO(result);

            var payload = new Payload<FollowDTO>() { Data = resultDTO };
            return TypedResults.Created(_path, payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Unfollow(IRepository<Follow> service, int id, ClaimsPrincipal user)
        {
            // get the id of the current user from the claims, that is stored in the token
            var userid = int.Parse(user.FindFirstValue(ClaimTypes.Sid)!);

            var follow = new Follow() { UserId = userid, OtherUserId = id };
            var result = await service.Delete(["User", "OtherUser"], follow);
            var resultDTO = new UnfollowDTO(result);

            var payload = new Payload<UnfollowDTO>() { Data = resultDTO };
            return TypedResults.Created(_path, payload);
        }
    }
}
