using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;

namespace exercise.wwwapi.EndPoints
{
    public static class BlogPostApi
    {
        public static void ConfigureBlogPostApi(this WebApplication app)
        {
            var posts = app.MapGroup("posts");
            posts.MapGet("/GetAll", GetAllPosts);
            posts.MapPost("/Create", CreatePost);
            posts.MapGet("/Edit{id}", EditPost);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllPosts(IDatabaseRepository<BlogPost> repository)
        {
            //Get all posts
            return Results.Ok(repository.GetAll());
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(BlogRequestDTO request, IDatabaseRepository<BlogPost> repository, IHttpContextAccessor httpContextAccessor)
        {
            //Check if the user is logged in
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //Create a new blog post
            var blogPost = new BlogPost()
            {
                UserId = int.Parse(userId),
                Post = request.Text
            };

            //Add it to the database
            repository.Insert(blogPost);
            repository.Save();

            //Create payload
            var payload = new Payload<BlogPost>()
            {
                data = blogPost
            };

            //Response
            return Results.Created($"https://localhost:5005/posts/{payload.data.Id}", payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> EditPost(int id, BlogRequestDTO request, IDatabaseRepository<BlogPost> repository, IHttpContextAccessor httpContextAccessor)
        {
            //Check if the user is logged in
            var userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //Get the old blogpost
            var blogPost = repository.GetById(id);
            if(blogPost == null)
            {
                return Results.NotFound();
            }

            //Check if the userId equals the signed in userId
            if (blogPost.UserId != int.Parse(userId))
            {
                return Results.Unauthorized();
            }

            //Change the post and update the database
            blogPost.Post = request.Text;
            repository.Update(blogPost);
            repository.Save();

            //Create payload
            var payload = new Payload<BlogPost>()
            {
                data = blogPost
            };

            //Response
            return Results.Created($"https://localhost:5005/posts/{payload.data.Id}", payload);
        }
    }
}
