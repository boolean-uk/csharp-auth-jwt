using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Requests;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoint
    {
        public static void ConfigureBlogEndpoint(this WebApplication app)
        {
            var blogs = app.MapGroup("blogs");

            blogs.MapGet("/", GetAll);
            blogs.MapPost("/{id}", Add);
            blogs.MapPut("/{id}", Update);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var blogs = await repository.GetAll();
            List<Object> result = new List<Object>();

            foreach (var blogPost in blogs.OrderBy(x => x.Id))
            {
                result.Add(new BlogPost { Id = blogPost.Id, Text = blogPost.Text, AuthorId = blogPost.AuthorId });
            }

            return TypedResults.Ok(result);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> Add(IRepository<BlogPost> repository, ClaimsPrincipal user, BlogPostRequest blogPost)
        {
            if (blogPost == null)
            {
                return TypedResults.BadRequest("Invalid input: blog text required");
            }

            var entity = new BlogPost { Text = blogPost.Text, AuthorId = user.UserId() };
            var result = await repository.Add(entity);

            return TypedResults.Created($"/{result.Id}", new { result.Id, result.Text, result.AuthorId });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public static async Task<IResult> Update(IRepository<BlogPost> repository, ClaimsPrincipal user, int id, BlogPostRequest blogPost)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Blog post with id {id} was not found");
            }

            if (!user.IsInRole("Admin") && user.UserId() != entity.AuthorId)
            {
                return TypedResults.Unauthorized();
            }

            entity.Text = blogPost.Text;
            var result = await repository.Update(entity);

            if (result == null)
            {
                return TypedResults.BadRequest("Could not save to database");
            }

            return TypedResults.Created($"/{entity.Id}", new { entity.Id, entity.Text, entity.AuthorId });
        }
    }
}
