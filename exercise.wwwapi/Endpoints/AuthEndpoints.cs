using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Requests;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;
using System.Security.Claims;
using exercise.wwwapi.Helpers;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthenticationEndpoint(this WebApplication app)
        {
            var blogs = app.MapGroup("blogs");
            blogs.MapGet("/", GetAll);
            blogs.MapPost("/", AddBlog).AddEndpointFilter(async (invocationContext, next) =>
            {
                var addblog = invocationContext.GetArgument<BlogPostRequest>(1);

                if (string.IsNullOrEmpty(addblog.Text))
                {
                    return Results.BadRequest("You must add text");
                }
                return await next(invocationContext);
            }); ;
            blogs.MapPut("/", UpdateBlog);

        }

        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetAll(IRepository<Blogpost> repository)
        {
            return TypedResults.Ok(await repository.GetAll());
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> AddBlog(IRepository<Blogpost> repository, BlogPostRequest model, ClaimsPrincipal user)
        {
            var results = await repository.GetAll();

            if (results.Any(x => x.Text.Equals(model.Text, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("Blog with provided text already exists");
            }

            var entity = new Blogpost() { Text = model.Text, AuthorId = user.UserId() };
        
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.id}", new { Text = entity.Text, AuthorId = entity.AuthorId });
        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> UpdateBlog(IRepository<Blogpost> repository, int id, BlogPostRequest model)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Blog with Id:{id}");
            }
            entity.Text = !string.IsNullOrEmpty(model.Text) ? model.Text : entity.Text;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { Text = result.Text }) : TypedResults.BadRequest("Couldn't save to the database?!");
        }
    }
}
