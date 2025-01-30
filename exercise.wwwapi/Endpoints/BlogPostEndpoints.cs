using System.Security.Claims;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Exceptions;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Npgsql.TypeMapping;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoints
    {
        public static string Path { get; } = "posts";
        public static void ConfigureBlogPostEndpoints(this WebApplication app)
        {
            var group = app.MapGroup(Path);

            group.MapGet("/", GetPosts);
            group.MapPost("/", CreatePost);
            group.MapPut("{id}", UpdatePost);

            var user = app.MapGroup("user");
            user.MapPost("/{fromFollowId}/follows/{toFollowId}", StartFollowing);
            user.MapPost("/{fromFollowId}/unfollows/{toFollowId}", StopFollowing);
            app.MapGet("/viewall/{userId}", GetPostByFollowedUser);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> GetPosts(IRepository<BlogPost, int> repository)
        {
            try
            {
                IEnumerable<BlogPost> posts = await repository.GetAll();
                return TypedResults.Ok(new Payload { Data = posts.Select(x => new BlogPostView(x.Id, x.Text)) });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> CreatePost(
            IRepository<BlogPost, int> repository,
            ClaimsPrincipal user,
            BlogPostPost entity)
        {
            try
            {
                BlogPost blogPost = await repository.Add(new BlogPost
                {
                    Text = entity.Text,
                    UserId = user.UserRealId()!.Value
                });
                return TypedResults.Created($"{Path}/{blogPost.Id}", new Payload
                {
                    Data = new BlogPostView(blogPost.Id, blogPost.Text)
                });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> UpdatePost(
            IRepository<BlogPost, int> repository,
            ClaimsPrincipal user,
            BlogPostPut entity,
            int id)
        {
            try
            {
                BlogPost blogPost = await repository.Get(id);
                if (blogPost.UserId != user.UserRealId())
                {
                    return TypedResults.BadRequest(new Payload { Status = "Unauthorized", Data = "You are not the author of this post!" });
                }
                if (entity.Text != null) blogPost.Text = entity.Text;
                await repository.Update(blogPost);
                return TypedResults.Ok(new Payload
                {
                    Data = new BlogPostView(blogPost.Id, blogPost.Text)
                });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> StartFollowing(
            IRepository<BlogPost, int> repository,
            IRepository<User, int> userRepository,
            IRepository<UserRelation, int> userRelationRepository,
            ClaimsPrincipal user,
            int fromFollowId,
            int toFollowId)
        {
            try
            {
                if (fromFollowId != user.UserRealId()) 
                    return TypedResults.BadRequest(new Payload { Status = "Unauthorized", Data = "You can only follow other users with your own account!" });
                if (fromFollowId == toFollowId)
                    return TypedResults.BadRequest(new Payload { Status = "failure", Data = "You cannot follow yourself!" });
                User follower = await userRepository.Get(fromFollowId, q => q.Include(x => x.Following));
                User following = await userRepository.Get(toFollowId);
                if (follower.Following.Any(x => x.ToFollowId == toFollowId))
                    return TypedResults.BadRequest(new Payload { Status = "failure", Data = "You are already following this person!" });

                await userRelationRepository.Add(new UserRelation
                {
                    FromFollowId = follower.Id,
                    ToFollowId = following.Id,
                });

                return TypedResults.Ok(new Payload
                {
                    Data = $"You are now following '{following.UserName}!'"
                });
            }
            catch (EntityNotFoundException)
            {
                return TypedResults.NotFound(new Payload { Status = "failure", Data = "At least one of the provided IDs does not exist!" });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> StopFollowing(
            IRepository<BlogPost, int> repository,
            IRepository<User, int> userRepository,
            IRepository<UserRelation, int> userRelationRepository,
            ClaimsPrincipal user,
            int fromFollowId,
            int toFollowId)
        {
            try
            {
                if (fromFollowId != user.UserRealId())
                    return TypedResults.BadRequest(new Payload { Status = "Unauthorized", Data = "You can only stop following other users with your own account!" });
                if (fromFollowId == toFollowId)
                    return TypedResults.BadRequest(new Payload { Status = "failure", Data = "You cannot unfollow yourself!" });
                User follower = await userRepository.Get(fromFollowId, q => q.Include(x => x.Following));
                User following = await userRepository.Get(toFollowId);
                if (!follower.Following.Any(x => x.ToFollowId == toFollowId))
                    return TypedResults.BadRequest(new Payload { Status = "failure", Data = "You are not following this person!" });

                UserRelation ur = await userRelationRepository.Find(x => x.ToFollowId == toFollowId && x.FromFollowId == fromFollowId);
                await userRelationRepository.Delete(ur);

                return TypedResults.Ok(new Payload
                {
                    Data = $"You are no longer following '{following.UserName}'!"
                });
            }
            catch (EntityNotFoundException)
            {
                return TypedResults.NotFound(new Payload { Status = "failure", Data = "At least one of the provided IDs does not exist!" });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> GetPostByFollowedUser(
            IRepository<BlogPost, int> repository,
            IRepository<User, int> userRepository,
            IRepository<UserRelation, int> userRelationRepository,
            ClaimsPrincipal user,
            int userId)
        {
            try
            {
                User follower = await userRepository.Get(user.UserRealId()!.Value, q => q.Include(x => x.Following));
                if (userId != follower.Id && !follower.Following.Any(x => x.ToFollowId == userId))
                    return TypedResults.BadRequest(new Payload { Status = "failure", Data = "You are not following this person!" });
                User followedUser = await userRepository.Get(userId, q => q.Include(x => x.BlogPosts));

                return TypedResults.Ok(new Payload
                {
                    Data = followedUser.BlogPosts.Select(x => new BlogPostView(x.Id, x.Text))
                });
            }
            catch (EntityNotFoundException)
            {
                return TypedResults.NotFound(new Payload { Status = "failure", Data = "At least one of the provided IDs does not exist!" });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
