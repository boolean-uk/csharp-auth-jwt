using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataModels.Requests;
using exercise.wwwapi.DataModels.Response;
using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoint
{
    /// <summary>
    /// Endpoint for blog related operations.
    /// </summary>
    public static class BlogEndpoint
    {
        /// <summary>
        /// Configures the blog endpoints.
        /// </summary>
        public static void BlogEndpointConfiguration(this WebApplication app)
        {
            var posts = app.MapGroup("posts");

            posts.MapGet("", GetAll);
            posts.MapPost("", Create);
            posts.MapPut("{id}", Update);
        }

        /// <summary>
        /// Gets all blog posts.
        /// </summary>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        private static async Task<IResult> GetAll([FromServices] IRepository<BlogPost> repository)
        {
            IEnumerable<BlogPost> result = await repository.Get();

            IEnumerable<BlogPostOutput> output = BlogPostDtoManager.Convert(result);
            Payload<IEnumerable<BlogPostOutput>> payload = new Payload<IEnumerable<BlogPostOutput>>(output);
            return TypedResults.Ok(payload);
        }

        /// <summary>
        /// Creates a new blog post.
        /// </summary>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Create(ClaimsPrincipal user, [FromBody] BlogPostInput inputBlogPost, [FromServices] IRepository<BlogPost> repository)
        {
            BlogPost blogPost = new BlogPost
            {
                Text = inputBlogPost.Text,
                AuthorId = user.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            BlogPost result = await repository.Create(blogPost);

            BlogPostOutput output = BlogPostDtoManager.Convert(result);
            Payload<BlogPostOutput> payload = new Payload<BlogPostOutput>(output);
            return TypedResults.Created("url goes here", payload);
        }

        /// <summary>
        /// Updates a blog post. Only the original author or an admin can update a post.
        /// </summary>
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        private static async Task<IResult> Update(ClaimsPrincipal user, int id, [FromBody] BlogPostInput inputBlogPost, [FromServices] IRepository<BlogPost> repository)
        {
            BlogPost? blogPost = await repository.Get(id);
            if (blogPost == null)
                return TypedResults.NotFound();

            string userId = user.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (!user.IsInRole("Admin") && blogPost.AuthorId != userId)
                return TypedResults.Forbid();

            blogPost.Text = inputBlogPost.Text;

            BlogPost result = await repository.Update(blogPost);

            BlogPostOutput output = BlogPostDtoManager.Convert(result);
            Payload<BlogPostOutput> payload = new Payload<BlogPostOutput>(output);
            return TypedResults.Ok(payload);
        }
    }
}
