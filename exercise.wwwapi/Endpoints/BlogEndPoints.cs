using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{

    public record BlogPostPayload(string title, string body);
    public static class BlogEndPoints
    {
        public static void ConfigureBlogEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("blogpost");

            root.MapGet("/", GetAllPosts);
            root.MapPost("/", CreateBlogPost);
            root.MapPut("/{id}", UpdateBlogPost);
            root.MapDelete("/{id}", DeleteBlogPost);
        }

        public static async Task<IResult> GetAllPosts(IRepository repo )
        {
            var res = await repo.GetBlogPosts();
            return TypedResults.Ok(res);
        }

        [Authorize()]
        public static async Task<IResult> CreateBlogPost(IRepository repo, BlogPostPayload payload, ClaimsPrincipal user)
        {
            var data = new BlogPost() { Body = payload.body, Title = payload.title };
            var createdBlogPost = await repo.CreateBlog(data);
            return TypedResults.Created($"/blogpost/{createdBlogPost.Id}", createdBlogPost);
        }
        [Authorize()]
        public static async Task<IResult> UpdateBlogPost(IRepository repo,int id, BlogPostPayload payload)
        {
            var data = new BlogPost() { Body = payload.body, Title = payload.title };


          
            if (data == null)
            {
                return TypedResults.BadRequest("Invalid blog post data");
            }

            try
            {
                var updatedBlogPost = await repo.UpdateBlogPost(id, data);
                return TypedResults.Ok(updatedBlogPost);
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.NotFound($"Blog post with ID {id} not found");
            }
        }
        [Authorize()]
        public static async Task<IResult> DeleteBlogPost(IRepository repo, int id)
        {

            try
            {
                var deletedBlogPost = await repo.DeleteBlogPost(id);
                return TypedResults.Ok(deletedBlogPost);
            }
            catch (KeyNotFoundException)
            {
                return TypedResults.NotFound($"Blog post with ID {id} not found");
            }
        }
    }
}
