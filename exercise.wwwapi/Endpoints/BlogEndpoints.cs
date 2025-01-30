using api_cinema_challenge.Repository;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoints
    {
        public static void ConfigureBlogEndpoints(this WebApplication app)
        {
            var usergroup = app.MapGroup("/blogposts");
            usergroup.MapPost("/", CreatePost);
            usergroup.MapPut("/{id}", EditPost);
            usergroup.MapGet("/", GetAllPosts);
        }

        [Authorize]
        private static async Task<IResult> EditPost(HttpContext context, IRepository<User> repo)
        {
            return TypedResults.Ok();
        }

        [Authorize]
        private static async Task<IResult> GetAllPosts(HttpContext context, IRepository<User> repo)
        {
            return TypedResults.Ok();
        }

        [Authorize]
        private static async Task<IResult> CreatePost(HttpContext context, IRepository<User> repo)
        {
            return TypedResults.Ok();

        }
    }
}
