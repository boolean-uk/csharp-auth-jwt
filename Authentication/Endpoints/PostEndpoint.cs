using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Authentication.Helpers;
using Authentication.Model;
using Authentication.Repository;
using Authentication.DTO;
using Authentication.Enums;

namespace Authentication.Endpoints
{
    public record BlogPostPayload(string Text);
    public record BlogPostUpdatePayload(string? Text);

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
        public static IResult CreatePost(IRepository repo, BlogPostPayload newPostData, ClaimsPrincipal user)
        {
            if (newPostData.Text == null)
                return TypedResults.BadRequest("Text is required.");

            var userId = user.GetUserId();
            if (userId == null)
                return Results.Unauthorized();

            var post = repo.CreatePost(newPostData.Text, userId);
            var postResponse = new PostResponseDto(post.Id, post.Text);
            return TypedResults.Created($"/tasks{postResponse.id} {postResponse.text}", postResponse);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static IResult UpdatePost(IRepository repo, BlogPostUpdatePayload updatePayload, int id, ClaimsPrincipal user)
        {
            //Checking parameters
            var post = repo.GetPost(id);
            if (post == null)
                return TypedResults.BadRequest("Id out of scope");

            if (updatePayload.Text == null)
                return TypedResults.BadRequest("Text is required.");

            //Checking user identifications
            var userId = user.GetUserId();
            if (userId == null)
                return Results.Unauthorized();

            var repoUser = repo.GetUser(userId);
            if (repoUser == null)
                return Results.Unauthorized();

            if (userId != post.AuthorId && repoUser.Role != UserRole.Administrator)
                return Results.Unauthorized();

            //Updates the post
            post = repo.UpdatePost(post, updatePayload.Text);
            var postResponse = new PostResponseDto(post.Id, post.Text);
            return TypedResults.Created($"/tasks{postResponse.id} {postResponse.text}", postResponse);
        }
    }
}