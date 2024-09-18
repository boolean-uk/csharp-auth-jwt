
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfers.Requests;
using exercise.wwwapi.DataTransfers.Responses;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var blogs = app.MapGroup("blogs");
            blogs.MapGet("/posts", Get);
            blogs.MapPost("/posts{id}", Post).AddEndpointFilter(async (invocationContext, next) =>
            {
                var postblog = invocationContext.GetArgument<BlogPostInsert>(1);

                if (string.IsNullOrEmpty(postblog.Text))
                {
                    return Results.BadRequest("You must enter Blog text body");
                }
                return await next(invocationContext);
            }); ;
            blogs.MapPut("/posts{id}", Update);
        }
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        private static async Task<IResult> Get(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<BlogPost> results = new List<BlogPost>();
            foreach (var blogPost in entities)
            {
                results.Add(blogPost);
            }
            return TypedResults.Ok(results);
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Post(IRepository<BlogPost> repository, BlogPostInsert blog, ClaimsPrincipal user)
        {
            var results = await repository.Get();

            if(results.Any(x => x.Text.Equals(blog.Text, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("Blog post with this body text already exists, please don't spam");
            }
            var entity = new BlogPost() { Id = results.Count() + 1, Text =  blog.Text, AuthorId = user.UserId() };
            await repository.Insert(entity);
            return TypedResults.Created($"blog posted: {entity.Id}", entity);
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Update(IRepository<BlogPost> repository, int id, BlogPostUpdate blog, ClaimsPrincipal user)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find blog with provided Id:{id}");
            }

            if (user.IsInRole("Admin") || entity.AuthorId == user.UserId())
            {
                entity.Text = !string.IsNullOrEmpty(blog.Text) ? blog.Text : entity.Text;

                var result = await repository.Update(entity);

                return result != null ? TypedResults.Ok(new { Text = entity.Text}) : 
                    TypedResults.BadRequest("Failed to update blogpost");
                
            }

            return TypedResults.Unauthorized();

        }


    }
}
