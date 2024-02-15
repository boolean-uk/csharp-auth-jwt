using exercise.wwwapi.DTO;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.Enum;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using exercise.wwwapi.Model;
using exercise.wwwapi.Helpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;

namespace exercise.wwwapi.Endpoint
{
    public static class BlogEndpoint
    {
        public static void ConfigureBlogEndpoint(this WebApplication app)
        {
            var blog = app.MapGroup("/blogposts");
            blog.MapGet("/", GetAll);
            blog.MapPost("/", AddPost).AddEndpointFilter(async (invocationContext, next) =>
            {
                var post = invocationContext.GetArgument<BlogPostRequest>(1);

                if (string.IsNullOrEmpty(post.Title) || string.IsNullOrEmpty(post.Author))
                {
                    return Results.BadRequest("You must enter a Title AND Author");
                }
                return await next(invocationContext);
            });
            blog.MapGet("/{id}", GetById);
            blog.MapPut("/{id}", Update);
            blog.MapDelete("/{id}", Delete);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> Delete(IRepository<BlogPost> repository, ClaimsPrincipal user, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find post with Id:{id}");
            }
            var result = await repository.Delete(entity);
            return result != null ? TypedResults.Ok(new { DateTime = DateTime.Now, User = user.Email(), BlogPost = new { title = result.Title, text = result.Text, author = result.Author } }) : TypedResults.BadRequest($"Post wasn't deleted");
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<BlogPost> repository, int id, BlogPatchRequest post)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            entity.Title = !string.IsNullOrEmpty(post.Title) ? post.Title : entity.Title;
            entity.Text = !string.IsNullOrEmpty(post.Text) ? post.Text : entity.Text;
            entity.Author = !string.IsNullOrEmpty(post.Author) ? post.Author : entity.Author;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { Title = result.Title, Text = result.Text, Author = result.Author }) : TypedResults.BadRequest("Couldn't save to the database?!");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach (var post in entities)
            {
                results.Add(new { Id = post.Id, title = post.Text, text = post.Text, author = post.Author , createdAt = post.createdAt });
            }
            return TypedResults.Ok(results);
        }
        [Authorize(Roles = "User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<BlogPost> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            return TypedResults.Ok(new { title = entity.Title, text = entity.Text, author = entity.Author, createdAt = entity.createdAt });
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddPost(IRepository<BlogPost> repository, BlogPostRequest post)
        {
            var results = await repository.Get();

            if (results.Any(x => x.Title.Equals(post.Title, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("Post with provided name already exists");
            }

            var entity = new BlogPost() { Title = post.Text, Text = post.Text, Author = post.Author, createdAt = post.createdAt };
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", new { Title = entity.Title, Text = entity.Text, Author = entity.Author, createdAt = entity.createdAt });

        }
    }
}
