
using exercise.wwwapi.DataModels;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndPoint(this WebApplication app)
        {
            var blogposts = app.MapGroup("blogposts");
            blogposts.MapGet("/", GetAllBlogPosts);
        }

        [Authorize]
        private static async Task<IResult> GetAllBlogPosts(IRepository<BlogPost> bpRepo)
        {
            return TypedResults.Ok(bpRepo.GetAll());
        }
    }
}
