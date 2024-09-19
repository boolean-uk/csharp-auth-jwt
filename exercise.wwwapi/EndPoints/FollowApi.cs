using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.EndPoints
{
    public static class FollowApi
    {
        public static void ConfigureFollowEndpoint(this WebApplication app)
        {
            var followGroup = app.MapGroup("follows");

            followGroup.MapPost("/Follow/{id}", FollowUser);
            followGroup.MapPost("/Unfollow/{id}", UnfollowUser);
            followGroup.MapGet("/ViewAll", GetPostsFromFollowings);
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPostsFromFollowings(IDatabaseRepository<BlogPost> blogService,
            ClaimsPrincipal claim, IDatabaseRepository<User> service)
        {
            // is user logged in?
            var userId = claim.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }
            
            var posts = blogService.GetAll();
            List<BlogResponse> result = new List<BlogResponse>();
            User user = service.GetById(userId);


            foreach (var post in posts)
            {
                if(user.Following.Contains(post.UserId))
                {
                    BlogResponse dto = new BlogResponse();
                    dto.user = post.User.Username;
                    dto.PostId = post.Id;
                    dto.PostText = post.BlogText; 
                    result.Add(dto);
                }
            }
            var payload = new Payload<List<BlogResponse>>();
            payload.data = result;


            return Results.Ok(payload);

        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> UnfollowUser(string userName, IDatabaseRepository<User> service, ClaimsPrincipal claim)
        {
            // is user logged in?
            var userId = claim.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //user does not exist
            if (!service.GetAll().Where(u => u.Username == userName).Any())
            {
                return Results.BadRequest(new Payload<string>() { data = $"User {userName} does not exist!" });
            }

            User userToUnfollow = service.GetAll().FirstOrDefault(u => u.Username == userName)!;
            User user = service.GetById(userId);
            
            if (!user.Following.Contains(userToUnfollow.Id))
            {
                return Results.NotFound($"user did not follow {userName}");
            }

            user.Following.Remove(userToUnfollow.Id);
            service.Update(user);
            service.Save();

            return Results.Ok(new Payload<string>() { data = $"You unfollowed {userToUnfollow.Username}!" });
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> FollowUser(string userName, IDatabaseRepository<User> service, ClaimsPrincipal claim)
        {
            // is user logged in?
            var userId = claim.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //user does not exist
            if (!service.GetAll().Where(u => u.Username == userName).Any())
            {
                return Results.NotFound(new Payload<string>() { data = $"User {userName} does not exist!" });
            }

            User userToFollow = service.GetAll().FirstOrDefault(u => u.Username == userName)!;
            User user = service.GetById(userId);

            if (user.Following.Contains(userToFollow.Id))
            {
                return Results.BadRequest(new Payload<string>() { data = $"User already follows {userName}!" });
            }

            user.Following.Add(userToFollow.Id);
            service.Update(user);
            service.Save();

            return Results.Ok(new Payload<string>() { data = $"You now follow {userToFollow.Username}!" });
        }
    }
}
