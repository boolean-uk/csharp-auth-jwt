using System.Security.Claims;
using exercise.wwwapi.Dto;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wwwapi.Dto;

namespace exercise.wwwapi.Endpoints;

public static class BlogApi
{
    public static void ConfigureBlogApi(this WebApplication app)
    {
        var blog = app.MapGroup("/posts");

        blog.MapGet("/", GetAllBlogPosts);
        blog.MapPost("/", CreateNewPost);
        blog.MapPut("/", UpdatePost);
    }

    [Authorize]
    private static async Task<IResult> UpdatePost([FromServices] IRepository<Blogpost> repo, ClaimsPrincipal user, [FromBody] BlogPostPut postPut)
    {
        try
        {
            var userRealId = user.UserRealId();
            var post = repo.GetById(postPut.PostId);
            if (post == null)
            {
                return Results.NotFound("Post not found");
            }
            if (userRealId.Value != post.AuthorId)
            {
                return Results.Unauthorized();
            }

            post.Text = postPut.Text;
            repo.Update(post);
            repo.Save(); // Save changes to the database
            return TypedResults.Ok(post);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(ex.Message);
        }
    }

    [Authorize]
    private static async Task<IResult> CreateNewPost([FromServices] IRepository<Blogpost> repo, [FromBody] BlogPostPost postPost)
    {
        try
        {
            var newPost = new Blogpost()
            {
                Text = postPost.Text,
                AuthorId = postPost.UserId
            };
            repo.Insert(newPost);
            repo.Save(); // Save changes to the database
            return TypedResults.Ok(newPost);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(ex.Message);
        }
    }

    [Authorize]
    private static async Task<IResult> GetAllBlogPosts([FromServices] IRepository<Blogpost> repo)
    {
        try
        {
            var posts = repo.GetAll(bp => bp.User); // Include the user property
            var postsDto = posts.Select(p => new BlogPostDto()
            {
                PostId = p.Id,
                Text = p.Text,
                Author = p.User.Username
            });
            return TypedResults.Ok(postsDto);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError(ex.Message);
        }
    }
}