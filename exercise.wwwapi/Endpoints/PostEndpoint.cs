using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class PostEndpoint
    {
        public static void PostEndpointConfiguration(this WebApplication app)
        {
            var students = app.MapGroup("posts");
            students.MapGet("/", GetPosts);
            students.MapPost("/", AddPost);
            students.MapPut("/{id}", UpdatePost);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetPosts(IRepository repository)
        {
            var results = await repository.GetPosts();
            var payload = new Payload<IEnumerable<Post>>() { data = results };
            return TypedResults.Ok(payload);
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddPost(IRepository repository, ClaimsPrincipal user, PostInput post)
        {
            if (!post.Text.Any()) return TypedResults.BadRequest();

            var result = await repository.AddPost(new Post { Text = post.Text, AuthorId = user.UserId() });
            Payload<Post> payload = new() { data = result };
            return TypedResults.Created($"{result.Id}", payload);
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> UpdatePost(IRepository repository, int id, ClaimsPrincipal user, PostInput postData)
        {
            var post = await repository.GetPost(id);
            if (post == null) return TypedResults.NotFound();

            if (user.IsInRole("User") && user.UserId() != post.AuthorId) return TypedResults.Unauthorized();

            if (!postData.Text.Any()) return TypedResults.BadRequest();

            post.Update(postData);

            var result = await repository.UpdatePost(post);
            Payload<Post> payload = new() { data = result };
            return TypedResults.Ok(payload);
        }

    }
}
