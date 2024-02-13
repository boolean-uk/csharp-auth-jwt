using exercise.wwwapi.auth.DTOs;
using exercise.wwwapi.auth.Models;
using exercise.wwwapi.auth.Repositories;
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
        }

        /// <summary>
        /// A AUTHERIZED user, admin or moderator can create a blog
        /// </summary>
        /// <param name="blogRepository"></param> class that access the db ad add the blof to the blogs table
        /// <param name="payload"></param> the data we need to create a blog
        /// <param name="context"></param> https://learn.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8.1
        /// <returns></returns> 201 if the blog is created, else 400 badrequest 
        [Authorize(Roles = "Admin, Moderator, User")]
        public static async Task<IResult> CreateBlog(IBlogRepository blogRepository, BlogPostPayload payload, HttpContext context)
        {
            var userId = context.User.GetUserId();
            if (userId == null) return TypedResults.BadRequest("You must be authorized");
            var userName = context.User.GetUserName();
            if (userName == null) return TypedResults.BadRequest();

            if (payload.Title == null || payload.Title == "") return TypedResults.BadRequest("Title is required");
            if (payload.Description == null || payload.Description == "") return TypedResults.BadRequest("Description is required");


            var result = await blogRepository.createBlog(userId, userName, payload.Title, payload.Description);
            if (result == null)
            {
                return TypedResults.BadRequest("Something went wrong");
            } else
            {
                return TypedResults.Created($"/blog ",new BlogDTO(result));
            }
        }

        
    }
}
