using exercise.wwwapi.DataModels;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using exercise.wwwapi.Helpers;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {

            var cars = app.MapGroup("blogposts");
            cars.MapGet("/", GetAll);
            cars.MapPost("/", AddBlogPost);
            cars.MapGet("/{id}", GetById);
            cars.MapPut("/{id}", Update);
            cars.MapDelete("/{id}", Delete);

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
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            var result = await repository.Delete(entity);
            return result != null ? TypedResults.Ok(new { DateTime = DateTime.Now, User = user.Email(), BlogPost = new { Text = result.Text, AuthorId = result.AuthorId } }) : TypedResults.BadRequest($"BlogPost wasn't deleted");
        }
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<BlogPost> repository, int id, ClaimsPrincipal user, BlogPostPatch input)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            if (user.IsInRole("Admin") || entity.AuthorId == user.UserId() )
            {
                entity.Text = !string.IsNullOrEmpty(input.Text) ? input.Text : entity.Text;

                var result = await repository.Update(entity);

                return result != null ? TypedResults.Ok(new { Text = result.Text}) : TypedResults.BadRequest("Couldn't save to the database?!");
            }
            return TypedResults.Unauthorized();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<BlogPostGet> results = new List<BlogPostGet>();
            foreach (var blogPost in entities)
            {
                results.Add(new BlogPostGet(blogPost));
            }
            return TypedResults.Ok(results);
        }
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<BlogPost> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            return TypedResults.Ok(new BlogPostGet(entity));
        }
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddBlogPost(IRepository<BlogPost> repository, ClaimsPrincipal user, BlogPostRequset input)
        {
            var entities = await repository.Get();

            var entity = new BlogPost(input);
            entity.Id = (entities.Count() == 0 ? 0 : entities.Max(e => e.Id) + 1);
            entity.PostingUser = user.Email();
            entity.AuthorId = user.UserId();
            repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", entity);

        }
    }
}
