using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Requests;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {

            var blogpost = app.MapGroup("blogposts");
            blogpost.MapGet("/", GetAll);
            blogpost.MapPost("/", AddBlogPost);
            blogpost.MapGet("/{id}", GetById);
            blogpost.MapPut("/{id}", Update);
            blogpost.MapDelete("/{id}", Delete);

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
            return result != null ? TypedResults.Ok(new { DateTime = DateTime.Now, User = user.Email(), BlogPost = new { Text = result.Text } }) : TypedResults.BadRequest($"Post wasn't deleted");
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<BlogPost> repository, ClaimsPrincipal user, int id, BlogPostPatchRequest model)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find post with Id:{id}");
            }
            if(user.IsInRole("User")) { return TypedResults.NotFound($"USer not allowed to update"); }

            entity.Text = !string.IsNullOrEmpty(model.Text) ? model.Text : entity.Text;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { Text = result.Text, AuthorId = result.AuthorId }) : TypedResults.BadRequest("Couldn't save to the database?!");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach (var post in entities)
            {
                results.Add(new { Id = post.Id, Text = post.Text, AuthorId = post.AuthorId });
            }
            return TypedResults.Ok(results);
        }
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<BlogPost> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Car with Id:{id}");
            }
            return TypedResults.Ok(new { Text = entity.Text, AuthorId = entity.AuthorId });
        }
        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddBlogPost(IRepository<BlogPost> repository, ClaimsPrincipal user, BlogPostPostRequest model)
        {
            var results = await repository.Get();
            string id = user.UserId();

            if (results.Any(x => x.Text.Equals(model.Text, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("Blog post with provided name already exists");
            }

            
            var entity = new BlogPost() { Text = model.Text };
            entity.AuthorId = id;
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", new { Text = model.Text, AuthorId = id });

        }
    }
}
