using exercise.wwwapi.DataModels;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void BlogPostEndpointConfiguration(this WebApplication app)
        {
            var blogPosts = app.MapGroup("blogpost");
            blogPosts.MapGet("/", GetBlogPosts);
            blogPosts.MapPost("/", CreateBlogPost);
            blogPosts.MapPut("/{id}", UpdateBlogPost);
            blogPosts.MapDelete("/{id}", DeleteBlogPost);
        }

        
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public static async Task<IResult> GetBlogPosts(IRepository<BlogPost> repository)
        {
            Payload<List<BlogPostDto>> output = new();
            output.data = new();
            foreach (BlogPost blogPost in await repository.Get())
            {
                output.data.Add(new BlogPostDto(blogPost));
            }
            return TypedResults.Ok(output);
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> CreateBlogPost(IRepository<BlogPost> repository, PostBlogPost newBlogPost, ClaimsPrincipal user)
        {
            Payload<BlogPostDto> output = new();

            var blogPosts = await repository.Get();
            BlogPost blogPost = new BlogPost(newBlogPost);
            blogPost.AuthorId = user.UserId();
            blogPost.Id = blogPosts.Last().Id + 1;

            output.data = new BlogPostDto(await repository.Create(blogPost));
            return TypedResults.Created($"/{output.data.Id}", output);
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> UpdateBlogPost(IRepository<BlogPost> repository, int id, PostBlogPost updateBlogPost, ClaimsPrincipal user)
        {
            Payload<BlogPostDto> output = new();

            BlogPost blogPost = await repository.GetById(id);
            if (blogPost == null)
            {
                output.status = "not found";
                return TypedResults.NotFound(output);
            }

            if (blogPost.AuthorId != user.UserId() && !user.IsInRole("Admin"))
            {
                output.status = "unauthorized";
                return TypedResults.NotFound(output);
            }

            blogPost.Text = updateBlogPost.Text ?? output.data.Text;

            output.data = new BlogPostDto(await repository.Update(blogPost));

            return TypedResults.Created($"/{output.data.Id}", output);

        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> DeleteBlogPost(IRepository<BlogPost> repository, int id, ClaimsPrincipal user)
        {
            Payload<BlogPostDto> output = new();
            BlogPost blogPost = await repository.GetById(id);
            if (blogPost == null)
            {
                output.status = "failed";
                return TypedResults.NotFound(output);
            }

            if (blogPost.AuthorId != user.UserId() && !user.IsInRole("Admin"))
            {
                output.status = "unauthorized";
                return TypedResults.NotFound(output);
            }

            output.data = new BlogPostDto(await repository.Delete(blogPost));
            if (output.data == null)
            {
                output.status = "failed";
                return TypedResults.NotFound(output);
            }
            return TypedResults.Ok(output);
        }
    }
}
