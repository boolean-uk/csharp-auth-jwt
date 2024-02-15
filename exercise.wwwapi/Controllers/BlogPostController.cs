using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Requests;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Controllers
{
    public static class BlogPostController
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {

            var blog = app.MapGroup("posts");
            blog.MapGet("/", GetAll);
            blog.MapPost("/", AddBlogPost);

            blog.MapGet("/{id}", GetById);
            blog.MapPut("/{id}", Update);
            blog.MapDelete("/{id}", Delete);



        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach (var blog in entities)
            {
                results.Add(new { Id = blog.Id, Text = blog.Text, AuthorId = blog.AuthorId, 
                    CreatedAt = blog.CreatedAt, UpdatedAt = blog.UpdatedAt });
            }
            return TypedResults.Ok(results);
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddBlogPost(IRepository<BlogPost> repository, BlogPostCreateRequest blog)
        {
            var results = await repository.Get();
            if (string.IsNullOrEmpty(blog.Text) || string.IsNullOrEmpty(blog.AuthorId))
            {
                return Results.BadRequest("You must enter a Text AND authorId");
            }

            if (results.Any(x => x.Text.Equals(blog.Text, StringComparison.OrdinalIgnoreCase)))
            {
                return Results.BadRequest("BlogPost with provided Text already exists");
            }

            var entity = new BlogPost() { Text = blog.Text, AuthorId = blog.AuthorId,
                CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };
            await repository.Insert(entity);
            return TypedResults.Created($"/{entity.Id}", entity);

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
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            return TypedResults.Ok(new { Text = entity.Text, AuthorId = entity.AuthorId });

        }


        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<BlogPost> repository, int id, BlogPostUpdateRequest blog)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find BlogPost with Id:{id}");
            }
            entity.Text = !string.IsNullOrEmpty(blog.Text) ? blog.Text : entity.Text;
            entity.AuthorId = !string.IsNullOrEmpty(blog.AuthorId) ? blog.AuthorId : entity.AuthorId;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { Text = result.Text, Model = result.AuthorId }) : 
                TypedResults.BadRequest("Couldn't save to the database?!");
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
            return result != null ? TypedResults.Ok(new { DateTime = DateTime.Now, User = user.Email(),
                     BlogPost = new { Text = result.Text, Author = result.AuthorId } }) : 
                     TypedResults.BadRequest($"BlogPost wasn't deleted");
        }
    }
}

