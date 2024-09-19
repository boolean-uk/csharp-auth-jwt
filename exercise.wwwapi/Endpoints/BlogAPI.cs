using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.Extensions;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogAPI
    {
        public static void ConfigureBlogAPI(this WebApplication app)
        {
            var blog = app.MapGroup("blog");
            blog.MapGet("/GetAllBlogPosts", GetAllPosts);
            blog.MapPost("/CreateBlogPost", CreatePost);
            //blog.MapPut("/EditBlogPost", EditBlogPost);
        }

        [Authorize]
        private static async Task<IResult> GetAllPosts(IDatabaseRepository<Post> repository)
        {
            var posts =  await repository.GetAll();
            var postsDTO = posts.Select(p => p.ToDTO());

            Payload<IEnumerable<PostDTO>> payload = new() { Status = "success", Data  = postsDTO };

            return TypedResults.Ok(payload);
        }

        [Authorize]
        private static async Task<IResult> CreatePost(IDatabaseRepository<Post> repository, PostCreate postCreate, ClaimsPrincipal claim)
        {
            var userId = claim.UserRealId();

            if (userId is null) return TypedResults.BadRequest("No user was found in request");

            Post post = new ()
            {
                UserId = (int) userId,
                Text = postCreate.Text
            };
            await repository.Insert(post);
            post = await repository.Reload(post);

            Payload<PostDTO> payload = new() { Status = "success", Data = post.ToDTO() };


            return TypedResults.Created("", payload);
        }

    }
}
