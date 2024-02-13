using System.Security.Claims;
using System.Security.Cryptography;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using static exercise.wwwapi.DTO.Payload;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoint
        {
        public static void ConfigureBlogpostEndpoint(this WebApplication app)
        {
            app.MapPost("blogposts", CreateBlogpost);
            app.MapGet("/blogposts", GetBlogposts);
        }
        public static async Task<IResult> CreateBlogpost(IRepository repository, CreateBlogpostPayload payload)
        {
            Blogpost blogpost = new Blogpost { Description = payload.Description,Title = payload.Title };
            var result = await repository.CreateBlogpost(blogpost);

            return TypedResults.Ok(result);
        }
        [Authorize()]
        public static async Task<IResult> GetBlogposts(IRepository repository, ClaimsPrincipal user)
        {
            string? roleType = user.UserRole();
            if (roleType == "Admin")
            {
                var result = await repository.GetPosts();
                return TypedResults.Ok(result);
            }
            else
            {
                return TypedResults.Forbid();
            }
        }
    }
}
