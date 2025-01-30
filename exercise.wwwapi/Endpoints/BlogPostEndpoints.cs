using System.Security.Claims;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoints
    {
        public static string Path { get; } = "posts";
        public static void ConfigureBlogPostEndpoints(this WebApplication app)
        {
            var group = app.MapGroup(Path);

            group.MapGet("/", GetPosts);
            group.MapPost("/", CreatePost);
            group.MapPut("{id}", UpdatePost);                
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> GetPosts(IRepository<BlogPost, int> repository)
        {
            try
            {
                IEnumerable<BlogPost> posts = await repository.GetAll();
                return TypedResults.Ok(new Payload { Data = posts.Select(x => new BlogPostView(x.Id, x.Text)) });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> CreatePost(
            IRepository<BlogPost, int> repository,
            ClaimsPrincipal user,
            BlogPostPost entity)
        {
            try
            {
                BlogPost blogPost = await repository.Add(new BlogPost
                {
                    Text = entity.Text,
                    UserId = user.UserRealId()!
                });
                return TypedResults.Created($"{Path}/{blogPost.Id}", new Payload
                {
                    Data = new BlogPostView(blogPost.Id, blogPost.Text)
                });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> UpdatePost(
            IRepository<BlogPost, int> repository,
            ClaimsPrincipal user,
            BlogPostPut entity,
            int id)
        {
            try
            {
                BlogPost blogPost = await repository.Get(id);
                if (blogPost.UserId != user.UserRealId())
                {
                    return TypedResults.BadRequest(new Payload { Status = "Unauthorized", Data = "You are not the author of this post!" });
                }
                if (entity.Text != null) blogPost.Text = entity.Text;
                await repository.Update(blogPost);
                return TypedResults.Ok(new Payload
                {
                    Data = new BlogPostView(blogPost.Id, blogPost.Text)
                });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
