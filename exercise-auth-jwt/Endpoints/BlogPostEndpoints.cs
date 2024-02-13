using exercise_auth_jwt.DataModels;
using exercise_auth_jwt.DTO;
using exercise_auth_jwt.Enum;
using exercise_auth_jwt.Helpers;
using exercise_auth_jwt.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise_auth_jwt.Endpoints
{
    public static class BlogPostEndpoints
    {
        public static void ConfigureBlogpostEndpoints(this WebApplication app)
        {
            var Blog = app.MapGroup("posts");
            Blog.MapGet("/", GetAllPosts);
            Blog.MapPost("/", MakeAPost);
            Blog.MapPut("/{blogpostId}", EditPost);
        }

        [Authorize()]
        public static async Task<IResult> GetAllPosts(IRepository repository, ClaimsPrincipal user)
        {
            var userId = user.UserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }
            var results = await repository.GetAllPosts();
            var payload = new Payload<IEnumerable<BlogPost>>() { data = results };

            return TypedResults.Ok(payload);
        }

        [Authorize(Roles = "Admin,User")]
        public static async Task<IResult> MakeAPost(IRepository repository, BlogPostPayload postPayload, ClaimsPrincipal user)
        {
            var userId = user.UserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }
            var result = await repository.MakeAPost(postPayload.text, userId);
            var payload = new Payload<BlogPost>() { data = result };

            return TypedResults.Ok(payload);
        }

        [Authorize(Roles = "Admin")]
        public static async Task<IResult> EditPost(IRepository repository, BlogPostUpdateData updateData, ClaimsPrincipal user)
        {
            var userId = user.UserId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }
            if(updateData.postId <= 0)
            {
                return TypedResults.BadRequest("post id needs to be a possitive integer");
            }
            var result = await repository.EditPost(updateData.postId, updateData.text);
            var payload = new Payload<BlogPost>() { data = result };

            return TypedResults.Ok(payload);
        }




    }

}
