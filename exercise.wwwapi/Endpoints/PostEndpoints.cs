using exercise.wwwapi.DTO;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class PostEndpoints
    {
        public static void ConfigurePostEndpoint(this WebApplication app)
        {
            var postGroup = app.MapGroup("post");
            postGroup.MapGet("", GetPosts);
            postGroup.MapPost("", CreatePost);
            postGroup.MapGet("/owner", GetOwnPosts);
            postGroup.MapPut("/{id}", EditPost);
        }

        [Authorize()]
        public static async Task<IResult> GetPosts(IBlogRepository repository)
        {
            ICollection<BlogPost> posts = await repository.GetPosts();
            return TypedResults.Ok(posts);
        }

        [Authorize()]
        public static async Task<IResult> CreatePost(IBlogRepository repository, ClaimsPrincipal user, PostPayload payload)
        {
            if (payload.text is null) return TypedResults.BadRequest("Please enter some text");
            string? username = user.UserName();
            if (username is null) return TypedResults.Unauthorized();
            BlogPost post = await repository.CreatePost(username, payload.text);
            return TypedResults.Ok(post);
        }

        [Authorize()]
        public static async Task<IResult> GetOwnPosts(IBlogRepository repository, ClaimsPrincipal user)
        {
            string? username = user.UserName();
            if (username is null) return TypedResults.Unauthorized();
            ICollection<BlogPost> posts = await repository.GetOwnPosts(username);
            return TypedResults.Ok(posts);
        }

        [Authorize()]
        public static async Task<IResult> EditPost(IBlogRepository repository, ClaimsPrincipal user, int id, PostPayload payload)
        {
            if (payload.text is null) return TypedResults.BadRequest("Please enter some text");
            //Get post
            BlogPost? post = await repository.GetPostById(id);
            if (post is null) return TypedResults.NotFound("The post doesn't exist");
            //Check if user is an admin or the author of the post
            if (user.UserRole() == "Admin")
            {
                BlogPost result = await repository.EditPost(post, payload.text);
                return TypedResults.Ok(result);

            }
            if (user.UserRole() == "User" && post.Author == user.UserName())
            {
                BlogPost result = await repository.EditPost(post, payload.text);
                return TypedResults.Ok(result);
            }
            //If user has no permissions return 401:Unauthorized 
            //Otherwise edit the post and return it
            return TypedResults.Unauthorized();
        }



    }
}
