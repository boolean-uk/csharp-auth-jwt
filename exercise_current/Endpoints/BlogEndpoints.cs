using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoints
    {
        public static void ConfigureBlogEndpoints(this WebApplication app)
        {
            var blogGroup = app.MapGroup("blog");

            blogGroup.MapGet("/posts", GetAllPosts);
            blogGroup.MapPost("/posts{id}", CreatePost);
            blogGroup.MapPut("/posts{id}", UpdatePost);
            blogGroup.MapGet("/users{id}", GetUser);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static async Task<IResult> GetAllPosts(IRepository repository, ClaimsPrincipal user)
        {
            var userId = user.UserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }
            var posts = await repository.GetAllPosts();
            return TypedResults.Ok(posts);
        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> CreatePost(IRepository repository, PostPostPayload payload)
        {
            if(payload.AuthorId <= 0 || payload.Text == null)
            {
                return TypedResults.BadRequest("You must fill all fields");
            }
            var post = await repository.CreatePost(payload.Text, payload.AuthorId);
            if (post == null)
            {
                return TypedResults.NotFound($"Could not create post");
            }
            return TypedResults.Created($"/posts{post.Id}", post);
        }
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> UpdatePost(IRepository repository, int id, PutPostPayload payload)
        {
            if (payload.AuthorId <= 0 || payload.Text == null)
            {
                return TypedResults.BadRequest("You must fill all fields");
            }
            var post = await repository.UpdatePost(id, payload.Text, payload.AuthorId);
            if (post == null)
            {
                return TypedResults.NotFound($"No post with id: {id} found");
            }
            return TypedResults.Created($"/posts{post.Id}", post);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetUser(IRepository repository, int id)
        {
            var user = await repository.GetUser(id);
            if (user == null)
            {
                return TypedResults.NotFound($"No such user found");
            }
            return TypedResults.Ok(user);
        }
    }
}
