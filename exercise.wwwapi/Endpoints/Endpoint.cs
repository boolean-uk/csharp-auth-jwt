using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;
using exercise.wwwapi.Repository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Requests;

namespace exercise.wwwapi.Endpoints
{
    public static class Endpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {

            var blogpost = app.MapGroup("blogpost");
            blogpost.MapGet("/", GetAll);
            blogpost.MapGet("/{id}", GetById);
            blogpost.MapPost("/", AddBlogPost).AddEndpointFilter(async (invocationContext, next) =>
            {
                var blogpost = invocationContext.GetArgument<BlogPostPostRequest>(1);

                if (string.IsNullOrEmpty(blogpost.Text))
                {
                    return Results.BadRequest("Text Cannot Be Blank");
                }
                return await next(invocationContext);
            }); ;
           
            blogpost.MapPut("/{id}", Update);
        

        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<blogPost> repository)
        {
            var entities = await repository.GetAll();
            List<Object> results = new List<Object>();
            foreach (var blogpost in entities)
            {
                results.Add(new { Id = blogpost.Id, text = blogpost.Text, AuthorId = blogpost.AuthorId });
            }
            return TypedResults.Ok(results);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<blogPost> repository, int id, BlogPostPostRequest Text)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            entity.Text = !string.IsNullOrEmpty(Text.Text) ? Text.Text : Text.Text;
            

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { Text = result.Text, }) : TypedResults.BadRequest("Couldn't save to the database?!");
        }
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<blogPost> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            return TypedResults.Ok(new { Text = entity.Text, AuthorId = entity.AuthorId });
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddBlogPost(IRepository<blogPost> repository, BlogPostPostRequest text)
        {
            var results = await repository.GetAll();

            if (results.Any(x => x.Text.Equals(text.Text, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("BlogPost with provided text already exists");
            }

            var entity = new blogPost() { Text = text.Text};
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", new { text = text.Text});

        }

    }
}
