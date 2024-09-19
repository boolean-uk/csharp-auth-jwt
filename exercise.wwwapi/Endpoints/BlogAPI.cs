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
            var posts = app.MapGroup("posts");
            posts.MapGet("/", GetAllPosts);
            posts.MapPost("/", CreatePost);
            posts.MapPut("/", EditBlogPost);
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

            return TypedResults.Created("Succesfully created blogpost");
        }

        [Authorize]
        private static async Task<IResult> EditBlogPost(IDatabaseRepository<Post> repository, PostUpdate postUpdate, int postId)
        {
            var post = await repository.GetById(postId);
            if (post is null) return TypedResults.BadRequest("That post doesn't exist");

            post.Update(postUpdate);

            await repository.Update(post);

            return TypedResults.Created("localhost:5005/posts/");
        }


    }
}
