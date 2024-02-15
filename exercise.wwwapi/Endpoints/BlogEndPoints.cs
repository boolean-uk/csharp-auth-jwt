using exercise.wwwapi.Helpers;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
            root.MapGet("/me", GetSpecificUserPosts);
            root.MapPost("/", CreateBlogPost);
            root.MapPut("/{id}", UpdateBlogPost);
            root.MapDelete("/{id}", DeleteBlogPost);
        }

        public static async Task<IResult> GetAllPosts(IRepository repo)
        {
            var res = await repo.GetBlogPosts();
            return TypedResults.Ok(res);
        }
        public static async Task<IResult> GetSpecificUserPosts(IRepository repo, ClaimsPrincipal user, ClaimsIdentity claimsIdentity)
        {
            
            List<BlogPost> res = await repo.GetBlogPosts();

            res = res.Where(b => b.UserId == user.UserId()).ToList();

            return TypedResults.Ok(res);
        }

        [Authorize()]
        public static async Task<IResult> CreateBlogPost(IRepository repo, BlogPostPayload payload, ClaimsPrincipal user)
        {
            var data = new BlogPost() { Body = payload.body, Title = payload.title, UserId = user.UserId() };
            BlogPost createdBlogPost = await repo.CreateBlog(data);

            return TypedResults.Created($"/blogpost/{createdBlogPost.Id}", createdBlogPost);
        }
        [Authorize()]
        public static async Task<IResult> UpdateBlogPost(IRepository repo, int id, BlogPostPayload payload, ClaimsPrincipal user)
        {
            var data = new BlogPost() { Body = payload.body, Title = payload.title };
            List<BlogPost> BlogPosts = await repo.GetBlogPosts();
            BlogPost? BlogPostToUpdate = BlogPosts.Find(b => b.Id == id);
            if (BlogPostToUpdate == null) return TypedResults.NotFound();

            if (!(user.UserRole() == "Admin") && !(user.UserId() == BlogPostToUpdate.UserId)) { return TypedResults.Unauthorized(); }

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
        public static async Task<IResult> DeleteBlogPost(IRepository repo, int id, UserManager<ApplicationUser> userManager, ClaimsPrincipal user)
        {
            List<BlogPost> BlogPosts = await repo.GetBlogPosts();
            BlogPost? BlogPostToDelete = BlogPosts.Find(b => b.Id == id);
            if (BlogPostToDelete == null) return TypedResults.NotFound();



            if (!(user.UserRole() == "Admin") && !(user.UserId() == BlogPostToDelete.UserId)) { return TypedResults.Unauthorized(); }

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
