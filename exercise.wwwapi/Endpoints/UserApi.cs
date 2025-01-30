using AutoMapper;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Extensions;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.EndPoints
{
    public static class UserEndpoints
    {
        public static void ConfigureUserApi(this WebApplication app)
        {
            var group = app.MapGroup("users");

            group.MapGet("/viewall", GetFollowingPosts);
            group.MapGet("/posts", GetPosts);
            group.MapPost("/posts", CreatePost);
            group.MapPut("/posts", EditPost);
            group.MapPost("/{id}/follows/{id2}", FollowUser);
            group.MapPost("/{id}/unfollows/{id2}", UnFollowUser);
            group.MapPost("/blogposts/{id}/comments", AddComment);

        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<BlogPost> blogPostRepo, IMapper mapper, ClaimsPrincipal user)
        {
            var blogPosts = await blogPostRepo.FindAll(b => b.Comments.Count > 0, b => b.Author, b => b.Comments);

            if (!blogPosts.Any())
            {
                return Results.NotFound(new Payload<string>
                {
                    status = Enums.ApiStatus.NotFound.ToString(),
                    data = "No posts found"
                });
            }

            return TypedResults.Ok(new Payload<IEnumerable<BlogPostDTO>>() { status = Enums.ApiStatus.Success.ToString(), data = mapper.Map<IEnumerable<BlogPostDTO>>(blogPosts) });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetFollowingPosts(IRepository<User> service, IRepository<UserFollower> userFollowerRepo, IRepository<BlogPost> blogPostRepo, IMapper mapper, ClaimsPrincipal user)
        {

            var following = await userFollowerRepo.FindAll(u => u.FollowerId == user.UserRealId(), u => u.Followee);

            if (!following.Any())
            {
                return Results.NotFound(new Payload<string>() { status = Enums.ApiStatus.NotFound.ToString(), data = $"Couldn't find any followers" });
            }

            var followeeUsers = following.Select(u => u.Followee).ToList();
            var blogPosts = await blogPostRepo.FindAll(bp => followeeUsers.Select(f => f.Id).Contains(bp.Author.UserId),bp => bp.Author,bp => bp.Comments);

            if (!blogPosts.Any())
            {
                return Results.NotFound(new Payload<string>
                {
                    status = Enums.ApiStatus.NotFound.ToString(),
                    data = "No posts found from followed users"
                });
            }

            return TypedResults.Ok(new Payload<IEnumerable<BlogPostDTO>>() { status = Enums.ApiStatus.Success.ToString(), data = mapper.Map<IEnumerable<BlogPostDTO>>(blogPosts) });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IRepository<Author> service, IRepository<BlogPost> blogPostRepo, IMapper mapper, ClaimsPrincipal user, CreatePost post)
        {
            Author author = await service.Get(u => u.UserId == user.UserRealId());

            if (author == null)
            {
                return Results.NotFound(new Payload<string>() { status = Enums.ApiStatus.NotFound.ToString(), data = $"Couldn't find author for user of id {user.UserRealId()}" });
            }

            BlogPost blogPost = new BlogPost
            {
                Text = post.Text,
                AuthorId = author.Id,
            };

            await blogPostRepo.Add(blogPost);

            return TypedResults.Ok(new Payload<BlogPostDTO>() { status = Enums.ApiStatus.Success.ToString(), data = mapper.Map<BlogPostDTO>(blogPost) });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> EditPost(IRepository<Author> service, IRepository<BlogPost> blogPostRepo, IMapper mapper, ClaimsPrincipal user, CreatePost post, int blogPostId)
        {
            Author author = await service.Get(u => u.UserId == user.UserRealId());
            BlogPost blogPost = await blogPostRepo.Get(b => b.Id == blogPostId);

            if (author.Id != blogPost.AuthorId && user.Role() != "Admin")
            {
                return Results.BadRequest(new Payload<string>() { status = Enums.ApiStatus.BadRequest.ToString(), data = $"You cannot edit posts that arent yours" });
            }

            if (blogPost == null)
            {
                return Results.NotFound(new Payload<string>() { status = Enums.ApiStatus.NotFound.ToString(), data = $"Couldn't find post of id {blogPostId}" });
            }

            blogPost.Text = post.Text;
            blogPost.UpdatedAt = DateTime.UtcNow;

            await blogPostRepo.Update(blogPost);

            return TypedResults.Ok(new Payload<BlogPostDTO>() { status = Enums.ApiStatus.Success.ToString(), data = mapper.Map<BlogPostDTO>(blogPost) });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> FollowUser(IRepository<User> service, IRepository<UserFollower> userFollowerRepo, IMapper mapper, ClaimsPrincipal user, int userId, int userId2)
        {
            if (user.UserRealId() != userId)
            {
                return Results.Unauthorized();
            }
            var follower = await service.Get(u => u.Id == userId);
            var followee = await service.Get(u => u.Id == userId2);

            if (follower == null || followee == null)
            {
                return Results.BadRequest(new Payload<string>() { status = Enums.ApiStatus.BadRequest.ToString(), data = "Invalid user IDs" });
            }

            var userFollowerCheck = await userFollowerRepo.Get(u => u.FollowerId == userId && u.FolloweeId == userId2);

            if (userFollowerCheck != null)
            {
                return Results.BadRequest(new Payload<string>() { status = Enums.ApiStatus.BadRequest.ToString(), data = $"User by id:{userId} is already follows user by id: {userId2}" });
            }

            var userFollower = new UserFollower
            {
                FollowerId = userId,
                FolloweeId = userId2
            };

            UserFollower newUserFollower = await userFollowerRepo.Add(userFollower);
            return TypedResults.Ok(new Payload<UserFollowerDTO>() { status = Enums.ApiStatus.Success.ToString(), data = mapper.Map<UserFollowerDTO>(userFollower) });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UnFollowUser(IRepository<User> service, IRepository<UserFollower> userFollowerRepo, IMapper mapper, ClaimsPrincipal user, int userId, int userId2)
        {
            if (user.UserRealId() != userId)
            {
                return Results.Unauthorized();
            }
            var follower = await service.Get(u => u.Id == userId);
            var followee = await service.Get(u => u.Id == userId2);

            if (follower == null || followee == null)
            {
                return Results.BadRequest(new Payload<string>() { status = Enums.ApiStatus.BadRequest.ToString(), data = "Invalid user IDs" });
            }

            var userFollower = await userFollowerRepo.Get(u => u.FollowerId == userId && u.FolloweeId == userId2);

            if (userFollower == null)
            {
                return Results.BadRequest(new Payload<string>() { status = Enums.ApiStatus.BadRequest.ToString(), data = $"User by id:{userId} is not following user by id: {userId2}" });
            }

            UserFollower deletedUserFollower = await userFollowerRepo.Delete(userFollower);
            return TypedResults.Ok(new Payload<UserFollowerDTO>() { status = Enums.ApiStatus.Success.ToString(), data = mapper.Map<UserFollowerDTO>(deletedUserFollower) });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> AddComment(IRepository<User> service, IRepository<BlogPost> blogPostRepo, IRepository<Comment> commentRepo, IMapper mapper, ClaimsPrincipal claimsUser, int blogId, AddComment comment)
        {
         
            var blogPost = await blogPostRepo.Get(u => u.Id == blogId);
            var user = await service.Get(u => u.Id == claimsUser.UserRealId());

            if (blogPost == null || user == null)
            {
                return Results.BadRequest(new Payload<string>() { status = Enums.ApiStatus.BadRequest.ToString(), data = "Invalid IDs" });
            }

            Comment newComment = new Comment
            {
                Text = comment.Text,
                BlogPostId = blogPost.Id,
                UserId = user.Id,
            };

            Comment addedComment = await commentRepo.Add(newComment);
            return TypedResults.Ok(new Payload<CommentDTO>() { status = Enums.ApiStatus.Success.ToString(), data = mapper.Map<CommentDTO>(addedComment) });
        }

    }
}

