using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using workshop.webapi.DataModels;
using workshop.webapi.DataTransfer.Requests;
using workshop.webapi.Repository;

namespace workshop.webapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var posts = app.MapGroup("posts");
            posts.MapGet("/", GetAll);
            posts.MapPost("/", AddPost);
            posts.MapPut("/{id}", Update).RequireAuthorization();
            posts.MapDelete("/{id}", Delete).RequireAuthorization();
            posts.MapGet("/author/{authorId}", GetByAuthorId); // New route for getByAuthorId
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetByAuthorId(IRepository<BlogPost> repository, string authorId)
        {
            var posts = await repository.GetByAuthorId(authorId);
            if (posts == null)
            {
                return TypedResults.NotFound($"No posts found for author with ID: {authorId}");
            }
            return TypedResults.Ok(posts);
        }

        [Authorize] // accepts both admin and regular users as long as logged in
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var posts = await repository.Get();
            return TypedResults.Ok(posts);
        }

        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddPost(IRepository<BlogPost> repository, ClaimsPrincipal user, BlogPostRequest model)
        {
            if (string.IsNullOrEmpty(model.Text))
            {
                return Results.BadRequest("Post text cannot be empty");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var post = new BlogPost { Text = model.Text, AuthorId = userId };

            await repository.Insert(post);
            return TypedResults.Created($"/{post.Id}", post);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<BlogPost> repository, ClaimsPrincipal user, int id, BlogPatchRequest model)
        {
            var post = await repository.GetById(id);
            if (post == null)
            {
                return TypedResults.NotFound($"Post with Id:{id} not found");
            }

            var isAdmin = user.IsInRole("Admin");
            var isAuthor = post.AuthorId == user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!isAdmin && !isAuthor)
            {
                return Results.Forbid();
            }

            if (!string.IsNullOrEmpty(model.Text))
            {
                post.Text = model.Text;
            }

            var updatedPost = await repository.Update(post);
            if (updatedPost == null)
            {
                return Results.BadRequest("Failed to update post");
            }

            return TypedResults.Ok(updatedPost);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(IRepository<BlogPost> repository, ClaimsPrincipal user, int id)
        {
            var post = await repository.GetById(id);
            if (post == null)
            {
                return TypedResults.NotFound($"Post with Id:{id} not found");
            }

            var isAdmin = user.IsInRole("Admin");
            var isAuthor = post.AuthorId == user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!isAdmin && !isAuthor)
            {
                return Results.Forbid();
            }

            var result = await repository.Delete(post);
            if (result == null)
            {
                return Results.BadRequest("Failed to delete post");
            }

            return TypedResults.Ok(result);
        }
    }
}
