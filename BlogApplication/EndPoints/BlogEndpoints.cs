using Microsoft.AspNetCore.Mvc;
using BlogApplication.Repository;
using BlogApplication.Models;
using BlogApplication.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BlogApplication.Utils;
using BlogApplication.Enums;


namespace BlogApplication.EndPoints
{
    public static class BlogEndpoints
    {

        public static void ConfigureBlogEndpoints(this WebApplication app)
        {
            var surgeryGroup = app.MapGroup("posts");

            surgeryGroup.MapGet("/", GetAllBlogPosts);
            surgeryGroup.MapGet("/{postId}", GetBlogPost);
            surgeryGroup.MapPut("/{postId}", UpdateBlogPost);
            surgeryGroup.MapPost("/", CreateBlogPost);

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static IResult GetAllBlogPosts(IBlogRepository repo, ClaimsPrincipal user)
        {

            var uid = user.UserId();
            string role = user.UserRole().ToString();
            var email = user.UserEmail();


            if (uid == null)
            {
                return Results.Unauthorized();
            }

            return TypedResults.Ok(repo.GetAllBlogPosts(uid, role, email));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize()]
        public static IResult GetBlogPost(IBlogRepository repo, string id, ClaimsPrincipal user)
        {
            var uid = user.UserId();

            if (uid == null)
            {
                return Results.Unauthorized();
            }

            BlogPost? post = repo.GetBlogPost(id);
            if (post == null)
            {
                return TypedResults.NotFound($"Post with id {id} not found.");
            }
            return TypedResults.Ok(post);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize()]
        public static IResult CreateBlogPost(IBlogRepository repo, BlogPostPostPayload newData, ClaimsPrincipal user)
        {

            var uid = user.UserId();
            var email = user.UserEmail();

            if (uid == null)
            {
                return Results.Unauthorized();
            }

            if (newData.Text == null) return TypedResults.BadRequest("Title is required.");
            BlogPost post = repo.AddBlogPost(newData.Text, uid, email);
            return TypedResults.Created($"/tasks{post.Id}", post);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin")]
        public static IResult UpdateBlogPost(IBlogRepository repo, string id, BlogPostUpdatePayload updateData, ClaimsPrincipal user)
        {
            var uid = user.UserId();

            if (uid == null)
            {
                return Results.Unauthorized();
            }

            try
            {
                BlogPost? post = repo.UpdateBlogPost(id, updateData);
                if (post == null)
                {
                    return TypedResults.NotFound($"Post with id {id} not found.");
                }
                return TypedResults.Ok(post);
            }
            catch (Exception e)
            {
                return TypedResults.BadRequest(e.Message);
            }
        }

    }
}
