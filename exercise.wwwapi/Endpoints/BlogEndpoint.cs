using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.DataTransfer.Request;
using Microsoft.Extensions.Hosting;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoint
    {
        public static void ConfigureBlogEndpoints(this WebApplication app)
        {
            var blogGroup = app.MapGroup("blog");
            blogGroup.MapGet("/posts/", GetAll);
            blogGroup.MapGet("/posts/{id}", GetSingle);
            blogGroup.MapPost("/posts/", CreatePost);
            blogGroup.MapPut("/posts/{id}", EditPost);
            blogGroup.MapDelete("/posts/{id}", Delete);
        }

        [Authorize]
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
            return result != null ? TypedResults.Ok(new { DateTime = DateTime.Now, User = user.Email(), BlogPost = new { Title = result.Title, Text = result.Text } }) : TypedResults.BadRequest($"Post wasn't deleted");
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> EditPost(IRepository<BlogPost> repository, int id, PatchModel model)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            entity.Title = !string.IsNullOrEmpty(model.Title) ? model.Title : entity.Title;
            entity.Text = !string.IsNullOrEmpty(model.Text) ? model.Text : entity.Text;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { Title = result.Title, Text = result.Text }) : TypedResults.BadRequest("Couldn't save to the database?!");

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreatePost(IRepository<BlogPost> repository, PostModel post)
        {
            var entity = await repository.Insert(new BlogPost() { Text = post.Text, Title=post.Title});

            return TypedResults.Created($"/{entity.Id}", new { entity.Title, entity.Text });
        }

        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach (var post in entities)
            {
                results.Add(new { Id = post.Id, Title = post.Title, Text = post.Text });
            }
            return TypedResults.Ok(results);
        }

        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetSingle(IRepository<BlogPost> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find Post with Id:{id}");
            }
            return TypedResults.Ok(new { Title = entity.Title, Text = entity.Text });
        }

    }
}
