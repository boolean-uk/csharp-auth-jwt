using wwwapi.Extensions;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using System.Net;
using Microsoft.EntityFrameworkCore;
using exercise.wwwapi.Payload;

namespace exercise.wwwapi.Endpoints
{
    public static class FollowEndpoints
    {
        public static void ConfigureFollowEndpoints(this WebApplication app)
        {
            var usergroup = app.MapGroup("/");
            usergroup.MapPost("user/{userId}/follows/{otherUserId}", CreateFollowing);
            usergroup.MapPost("user/{userId}/unfollows/{otherUserId}", RemoveFollowing);
            usergroup.MapGet("viewall/{userId}", GetAllUsersPosts);
        }

        [Authorize]
        private static async Task<IResult> GetAllUsersPosts(HttpContext context, IRepository<BlogPost> repo, IRepository<UserFollows> uf_repo, IRepository<User> user_repo, ClaimsPrincipal user, int userId)
        {
            // Prohibit other users from seeing who anothe user follows...
            if (int.Parse(user.FindFirst(ClaimTypes.Sid).Value) != userId) return Fail.Payload("Unauthorized access denied", HttpStatusCode.Unauthorized);

            return TypedResults.Ok(
                await Get_BlogPost.toPayload(
                    uf_repo,
                    x => x.Where(x => x.FollowerId == userId)
                             .Include(x => x.Following)
                             .ThenInclude(x => x.BlogPosts),
                    x => x.SelectMany(x => x.Following.BlogPosts)
                    ));
        }

        [Authorize]
        private static async Task<IResult> CreateFollowing(HttpContext context, IRepository<UserFollows> repo, ClaimsPrincipal user, Create_Following dto, int userId, int otherUserId)
        {
            try
            {
                UserFollows follow = await dto.Create(repo, user, userId, otherUserId);
                return TypedResults.Created(context.Get_endpointUrl<int>(follow.FollowingsId), Get_Follows.toPayload(follow));
            }
            catch (HttpRequestException ex)
            {
                return Fail.Payload(ex);
            }

        }
        [Authorize]
        private static async Task<IResult> RemoveFollowing(HttpContext context, IRepository<UserFollows> repo, ClaimsPrincipal user, Delete_Following dto, int userId, int otherUserId)
        {
            try
            {
                UserFollows follow = await dto.Delete(repo, user, userId, otherUserId);
                return TypedResults.Ok(Get_Follows.toPayload(follow));
            }
            catch (HttpRequestException ex)
            {
                return Fail.Payload(ex);
            }
        }

    }
}
