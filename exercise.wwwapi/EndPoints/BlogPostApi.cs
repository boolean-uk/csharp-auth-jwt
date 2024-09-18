using exercise.wwwapi.Configuration;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
            posts.MapPut("/Edit/{id}", EditPost);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllPosts(IDatabaseRepository<BlogPost> repository)
        {
            //Get all posts
            var blogposts = repository.GetAll();

            //Create responses
            List<BlogResponseDTO> result = new List<BlogResponseDTO>();
            foreach (var blogpost in blogposts)
            {
                result.Add(new BlogResponseDTO() { PostId = blogpost.Id, Username = blogpost.User.Username, Post = blogpost.Post });
            }

            //Create payload
            var payload = new Payload<List<BlogResponseDTO>>()
            {
                data = result
            };

            return Results.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> CreatePost(BlogRequestDTO request, IDatabaseRepository<BlogPost> repository, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //Create a new blog post
            var blogPost = new BlogPost()
            {
                UserId = (int)userId,
                Post = request.Text
            };

            //Add it to the database
            repository.Insert(blogPost);
            repository.Save();

            //Get the blogpost from the database
            var post = repository.GetAll().Where(b => b.UserId == blogPost.UserId).LastOrDefault();
            if(post == null)
            {
                return Results.NotFound();
            }

            //Create blog response
            var blogresponse = new BlogResponseDTO()
            {
                PostId = post.Id,
                Username = post.User.Username,
                Post = post.Post
            };

            //Create payload
            var payload = new Payload<BlogResponseDTO>()
            {
                data = blogresponse
            };

            //Response
            return Results.Created($"https://localhost:5005/posts/{payload.data.PostId}", payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> EditPost(int id, BlogRequestDTO request, IDatabaseRepository<BlogPost> repository, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
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
            if (blogPost.UserId != userId)
            {
                return Results.Unauthorized();
            }

            //Change the post and update the database
            blogPost.Post = request.Text;
            repository.Update(blogPost);
            repository.Save();

            //Create blog response
            var blogresponse = new BlogResponseDTO()
            {
                PostId = blogPost.Id,
                Username = blogPost.User.Username,
                Post = blogPost.Post
            };

            //Create payload
            var payload = new Payload<BlogResponseDTO>()
            {
                data = blogresponse
            };

            //Response
            return Results.Created($"https://localhost:5005/posts/{payload.data.PostId}", payload);
        }
    }
}
