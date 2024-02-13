using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication.Helpers;
using Authentication.Model;
using Authentication.Repository;

namespace Authentication.Endpoints
{
    public record BlogPostPayload(string Title, string Description, string Author);
    public record BlogPostUpdatePayload(string? Title, string? Description, string? Author);

    public static class BlogPostEndpoint
    {
        public static void ConfigurePostEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("posts");
            authGroup.MapGet("/", GetAllPosts);
            authGroup.MapPost("/", CreatePost);
            authGroup.MapPut("/{id}", UpdatePost);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static IResult GetAllPosts(IRepository repo)
        {
            return TypedResults.Ok(repo.GetAllPosts());
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static IResult CreatePost(IRepository repo, BlogPostPayload newTaskData)
        {
            if (newTaskData.Title == null) return TypedResults.BadRequest("Title is required.");
            if (newTaskData.Description == null) return TypedResults.BadRequest("Description is required.");
            if (newTaskData.Author == null) return TypedResults.BadRequest("Author is required.");

            var post = repo.CreatePost(newTaskData.Title, newTaskData.Description, newTaskData.Author);
            return TypedResults.Created($"/tasks{post.Id}", post);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static IResult UpdatePost(IRepository repo, BlogPostUpdatePayload updatePayload, int id)
        {
            if (updatePayload.Title == null) return TypedResults.BadRequest("Title is required.");
            if (updatePayload.Description == null) return TypedResults.BadRequest("Description is required.");
            if (updatePayload.Author == null) return TypedResults.BadRequest("Author is required.");

            var post = repo.UpdatePost(id, updatePayload.Title, updatePayload.Description, updatePayload.Author);
            if (post == null)
                return TypedResults.BadRequest("Post not found");

            return TypedResults.Created($"/tasks{post.Id}", post);
        }
    }
}