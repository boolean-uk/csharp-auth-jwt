using AutoMapper;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoints
    {
        public static void ConfigureBlogEndpoints(this WebApplication app)
        {
            var blogGroup = app.MapGroup("posts");

            blogGroup.MapPost("/", CreateBlogPost);
            blogGroup.MapGet("/", GetBlogPosts);
            blogGroup.MapGet("/{id}", GetBlogPostById);
            blogGroup.MapPut("/{id}", UpdateBlogPost);
            blogGroup.MapDelete("/{id}", DeleteBlogPost);
            blogGroup.MapDelete("/deleteall", DeleteAll);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreateBlogPost(HttpContext httpContext, IRepository<BlogPost> blogRepository, IMapper mapper, string title, string text)
        {
            var userId = Util.GetUserIdFromClaims(httpContext);
            if (userId == null)
            {
                return TypedResults.Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Title cannot be empty!" });
            }
            if (string.IsNullOrWhiteSpace(text))
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Description cannot be empty!" });
            }

            var blogPost = new BlogPost
            {
                UserId = userId.Value,
                Title = title,
                Text = text
            };

            var createdPost = await blogRepository.Insert(blogPost);
            var createdPostDto = mapper.Map<BlogPostDTO>(createdPost);
            return TypedResults.Created($"/posts/{createdPost.Id}", new { status = "success", data = createdPostDto });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetBlogPosts(IRepository<BlogPost> blogRepository, IMapper mapper)
        {
            var posts = await blogRepository.Get();
            var postsDtos = mapper.Map<List<BlogPostDTO>>(posts);
            return TypedResults.Ok(new { status = "success", data = postsDtos });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetBlogPostById(IRepository<BlogPost> blogRepository, IMapper mapper, int id)
        {
            var post = await blogRepository.GetById(id);
            if (post == null)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Blog post not found!" });
            }
            var postDto = mapper.Map<BlogPostDTO>(post);
            return TypedResults.Ok(new { status = "success", data = postDto });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public static async Task<IResult> UpdateBlogPost(HttpContext httpContext, IRepository<BlogPost> blogRepository, IMapper mapper, int id, string? title, string? description)
        {
            var userId = Util.GetUserIdFromClaims(httpContext);
            if (userId == null)
            {
                return TypedResults.Unauthorized();
            }

            var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var isAdmin = userRole == "Admin";

            var post = await blogRepository.GetById(id);
            if (post == null)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Blog post not found!" });
            }

            if (!isAdmin && post.UserId != userId.Value)
            {
                return TypedResults.Forbid();
            }

            if (!string.IsNullOrWhiteSpace(title))
            {
                post.Title = title;
            }
            if (!string.IsNullOrWhiteSpace(description))
            {
                post.Text = description;
            }

            var updatedPost = await blogRepository.Update(post);
            var updatedPostDto = mapper.Map<BlogPostDTO>(updatedPost);
            return TypedResults.Ok(new { status = "success", data = updatedPostDto });
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public static async Task<IResult> DeleteBlogPost(HttpContext httpContext, IRepository<BlogPost> blogRepository, IMapper mapper, int id)
        {
            var userId = Util.GetUserIdFromClaims(httpContext);
            if (userId == null)
            {
                return TypedResults.Unauthorized();
            }

            var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            var isAdmin = userRole == "Admin";

            var post = await blogRepository.GetById(id);
            if (post == null)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Blog post not found!" });
            }

            if (!isAdmin && post.UserId != userId.Value)
            {
                return TypedResults.Forbid();
            }

            var deletedPost = await blogRepository.Delete(post.Id);
            var deletedPostDto = mapper.Map<BlogPostDTO>(deletedPost);
            return TypedResults.Ok(new { status = "success", data = deletedPostDto });
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public static async Task<IResult> DeleteAll(HttpContext httpContext, IRepository<BlogPost> blogRepository)
        {
            var posts = await blogRepository.Get();

            if (posts == null || posts.Count() == 0)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "No blog posts found!" });
            }

            var deletedPosts = new List<BlogPost>();
            foreach (var post in posts)
            {
                var deletedPost = await blogRepository.Delete(post.Id);
                deletedPosts.Add(deletedPost);
            }

            return TypedResults.Ok(new { status = "success", message = $"{deletedPosts.Count} blog posts deleted." });
        }
    }
}
