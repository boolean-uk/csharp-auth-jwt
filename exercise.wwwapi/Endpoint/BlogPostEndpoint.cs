using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoint
{
    public static class BlogPostEndpoint
    {
        public static void BlogPostEndpointConfiguration(this WebApplication app)
        {
            var posts = app.MapGroup("posts");
            
            posts.MapGet("", GetAll);
            posts.MapPost("", Create);
            posts.MapPut("{id}", Update);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        private static async Task<IResult> GetAll([FromServices] IRepository<BlogPost> repository)
        {
            IEnumerable<BlogPost> result = await repository.Get();
            
            IEnumerable<BlogPostOutput> output = BlogPostDtoManager.Convert(result);
            Payload<IEnumerable<BlogPostOutput>> payload = new Payload<IEnumerable<BlogPostOutput>>(output);
            return TypedResults.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Create([FromBody] BlogPostInput inputBlogPost, [FromServices] IRepository<BlogPost> repository)
        {
            BlogPost blogPost = BlogPostDtoManager.Convert(inputBlogPost);

            BlogPost result = await repository.Create(blogPost);

            BlogPostOutput output = BlogPostDtoManager.Convert(result);
            Payload<BlogPostOutput> payload = new Payload<BlogPostOutput>(output);
            return TypedResults.Created("url goes here", payload);
        }

        //TODO: should only be editable by an Admin, or the user that is the original author of the blog post
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Update(int id, [FromBody] BlogPostInput inputBlogPost, [FromServices] IRepository<BlogPost> repository)
        {
            BlogPost blogPost = BlogPostDtoManager.Convert(inputBlogPost);
            blogPost.Id = id;

            BlogPost result = await repository.Update(blogPost);

            BlogPostOutput output = BlogPostDtoManager.Convert(result);
            Payload<BlogPostOutput> payload = new Payload<BlogPostOutput>(output);
            return TypedResults.Ok(payload);
        }
    }
}
