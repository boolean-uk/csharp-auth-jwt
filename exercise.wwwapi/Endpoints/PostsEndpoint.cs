using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using exercise.wwwapi.Helpers;
namespace exercise.wwwapi.Endpoints
{
    public static class PostsEndpoint
    {
        public static void ConfigureApi(this WebApplication app)
        {
            app.MapGet("/posts", GetAllPosts);
            app.MapPost("/posts", CreatePost);
            app.MapPut("/posts", UpdatePost);
        }
        //Automatically checks in the header of the request if a jwt token is included and if it is valid
        //if so, we retrieve posts, else we get unauthorized response
        [Authorize()]
        private static async Task<IResult> GetAllPosts(IRepository repository)
        {
            return TypedResults.Ok(PostsResponseDTO.FromRepository(await repository.GetPosts()));
        }

        [Authorize()]
        private static async Task<IResult> CreatePost(IRepository repository, PostsPostPayload payload, ClaimsPrincipal user)
        {
            //retrieves the user id from the token which is included from the header by the Authorize() method
            string? userId = user.UserId();
            if (userId == null)
            {
                return TypedResults.Unauthorized();
            }
            Posts post = await repository.CreatePost(payload.text, userId);
            return TypedResults.Ok(post);
        }

        [Authorize()]
        private static async Task<IResult> UpdatePost(IRepository repository, PostsUpdatePayload payload, ClaimsPrincipal user)
        {
            //retrieves the user id from the token which is included from the header by the Authorize() method
            string? userId = user.UserId();
            UserRole? role = user.Role();
            if (userId == null && role != UserRole.Admin)
            {
                return TypedResults.Unauthorized();
            }

            Posts? post = await repository.GetPost(payload.id);
            if (post == null)
            {
                return TypedResults.NotFound();
            }

            //To test If an admin makes this request 
            if (post.UserId == userId || role == UserRole.Admin)
            {
                Posts? updatedPost = await repository.UpdatePost(payload.id, payload.text);
                return TypedResults.Ok(updatedPost);
            }

            return TypedResults.BadRequest("need to be author of post or admin to update post");


        }
    }
}
