using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;


namespace exercise.wwwapi.Endpoints
{
    public static class SecureApi
    {
        public static void ConfigureSecureApi(this WebApplication app)
        {
            app.MapGet("message", GetMessage);
            app.MapGet("posts", GetPosts);
            app.MapPost("newpost", MakePost);
            app.MapPut("updatepost", UpdatePost);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetMessage(IRepository<User> service, ClaimsPrincipal user, ILogger logger)
        {
            logger.LogDebug(new string('*', 1000));
            return TypedResults.Ok(new { LoggedIn = true, UserId = user.UserRealId().ToString(), Email = $"{user.Email()}", Message = "Pulled the userid and email out of the claims" });
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<Post> service) 
        {
            return TypedResults.Ok(service.GetAll());
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> MakePost(IRepository<Post> service, string title, string content )
        {
            Post post = new Post { postTitle = title, content = content };
            service.Insert(post);
            service.Save();
            return TypedResults.Ok(service.GetAll());
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdatePost(IRepository<Post> service, int id, string title, string content)
        {
 
            Post post = service.GetById(id);
            post.postTitle = title;
            post.content = content;
            service.Save();
            return TypedResults.Ok(service.GetAll());
        }
    }
}
