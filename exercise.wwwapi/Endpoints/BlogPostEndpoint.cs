using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Request.Entities;
using exercise.wwwapi.DataTransfer.Response;
using exercise.wwwapi.DataTransfer.Response.Entities;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var blogPosts = app.MapGroup("blogposts");
            blogPosts.MapGet("/", GetAll);
            blogPosts.MapGet("/{id}", GetById);
            blogPosts.MapPost("/", Create);
            blogPosts.MapPut("/{id}", UpdateById);
            blogPosts.MapDelete("/{id}", DeleteById);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        private static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            IEnumerable<BlogPost> results = await repository.GetAll();
            List<BlogPostResponseDTO> resultDTOs = new List<BlogPostResponseDTO>();
            foreach (BlogPost blogPost in results)
            {
                resultDTOs.Add(new BlogPostResponseDTO(blogPost));
            }
            return TypedResults.Ok(new Payload<List<BlogPostResponseDTO>>(resultDTOs));
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> GetById(IRepository<BlogPost> repository, int id)
        {
            BlogPost? blogPost = await repository.GetById(id);
            if (blogPost == null) return TypedResults.NotFound($"No Blog Post with ID={id}");
            BlogPostResponseDTO response = new BlogPostResponseDTO(blogPost);
            return TypedResults.Ok(new Payload<BlogPostResponseDTO>(response));
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        private static async Task<IResult> Create(IRepository<BlogPost> blogPostRepository, IRepository<User> userRepository, ClaimsPrincipal user, BlogPostRequestDTO input)
        {
            string? userId = user.UserId();
            if (userId == null) return TypedResults.BadRequest("Unable to fetch login credentials..");
            BlogPost newBlogPost = await blogPostRepository.Insert(new BlogPost()
            {
                Title = input.Title,
                Content = input.Content,
                UserId = userId
            });
            newBlogPost.User = await userRepository.GetById(userId);
            BlogPostResponseDTO response = new BlogPostResponseDTO(newBlogPost);
            return TypedResults.Created($"/{newBlogPost.Id}", new Payload<BlogPostResponseDTO>(response));
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> UpdateById(IRepository<BlogPost> repository, ClaimsPrincipal user, int id, BlogPostRequestDTO input)
        {
            string? userId = user.UserId();
            if (userId == null) return TypedResults.BadRequest("Unable to fetch login credentials..");
            BlogPost? blogPost = await repository.GetById(id);
            if (blogPost == null) return TypedResults.NotFound($"No Blog Post With ID={id}");
            if (blogPost.UserId != userId) return TypedResults.Unauthorized();
            blogPost.Title = input.Title;
            blogPost.Content = input.Content;
            blogPost.MarkUpdated();
            await repository.Update(blogPost);
            BlogPostResponseDTO response = new BlogPostResponseDTO(blogPost);
            return TypedResults.Created($"/{id}", new Payload<BlogPostResponseDTO>(response));
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteById(IRepository<BlogPost> repository, ClaimsPrincipal user, int id)
        {
            string? userId = user.UserId();
            if (userId == null) return TypedResults.BadRequest("Unable to fetch login credentials.");
            BlogPost? blogPost = await repository.GetById(id);
            if (blogPost == null) return TypedResults.NotFound($"No Blog Post With ID={id}");
            if (blogPost.UserId != userId) return TypedResults.Unauthorized();
            blogPost = await repository.DeleteById(id);
            BlogPostResponseDTO response = new BlogPostResponseDTO(blogPost);
            return TypedResults.Ok(new Payload<BlogPostResponseDTO>(response));
        }
    }
}
