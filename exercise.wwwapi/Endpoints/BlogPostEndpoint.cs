using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Requests;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var blogPosts = app.MapGroup("blogPosts");
            blogPosts.MapGet("/", GetAll);
            blogPosts.MapPost("/", AddBlogPost);
            blogPosts.MapGet("/{id}", GetById);
            blogPosts.MapPut("/{id}", Update);
            blogPosts.MapDelete("/{id}", Delete);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Delete(IRepository<BlogPost> repository, ClaimsPrincipal user, int id)
        {
            var entity = await repository.GetById(id);
            if(entity == null)
            {
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            var result = await repository.Delete(entity);
            return result != null ? TypedResults.Ok(new { DateTime = DateTime.Now, User = user.Email(), BlogPost = new { Text = entity.Text, AuthorId = entity.AuthorId  } }) : TypedResults.BadRequest($"BlogPost wasn't deleted");
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<BlogPost> repository, int id, BlogPostPatchRequest model)
        {
            var entity = await repository.GetById(id);
            if(entity == null)
            {
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            entity.Text = !string.IsNullOrEmpty(model.Text) ? model.Text : entity.Text;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { Text = result.Text }) : TypedResults.BadRequest("Couldn't save to the database");

        }

        [Authorize(Roles = "Admin")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach(var entity in entities)
            {
                results.Add(new { Id = entity.Id, Text = entity.Text });
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
            if(entity == null)
            {
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            return TypedResults.Ok(new {Id = entity.Id, Text = entity.Text});
        }

        [Authorize (Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddBlogPost(IRepository<BlogPost> repository, ClaimsPrincipal user, BlogPostPostRequest model)
        {
            var results = await repository.Get();

            var entity = new BlogPost() { Text = model.Text, AuthorId = user.UserId()};
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", new { Text = entity.Text});
            
        }
    }
}
