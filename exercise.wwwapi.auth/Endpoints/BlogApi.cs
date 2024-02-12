using exercise.wwwapi.auth.Models;
using Microsoft.AspNetCore.Identity;
using System.Web.Http;
using static exercise.wwwapi.auth.Payloads.BlogPayloads;

namespace exercise.wwwapi.auth.Endpoints
{
    public static class BlogApi
    {

        public static void ConfigureBlogApi(this WebApplication app)
        {
            var blogGroup = app.MapGroup("/blog");
            blogGroup.MapPost("/", CreateBlog);
            blogGroup.MapPost("/", UpdateBlog);
        }


        [Authorize()]
        public static async Task<IResult> CreateBlog(UserManager<ApplicationUser> userManager, BlogPostPayload payload)
        {
            var currentUser = await userManager.GetUserAsync(HttpContext.User); // <-- may work


            throw new NotImplementedException();
        }

        public static async Task<IResult> UpdateBlog()
        {

            throw new NotImplementedException();
        }
    }
}
