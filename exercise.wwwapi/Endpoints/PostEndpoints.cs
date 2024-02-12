using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;

namespace exercise.wwwapi.Endpoints
{
    public static class PostEndpoints
    {
        public static void ConfigurePostEndpoint(this WebApplication app)
        {
            var postGroup = app.MapGroup("post");
            postGroup.MapGet("", GetPosts);
        }

        public static async Task<IResult> GetPosts(IBlogRepository repository)
        {
            ICollection<BlogPost> posts = await repository.GetPosts();
            return TypedResults.Ok(posts);
        }
    }
}
