using exercise.minimalapi.DTOs;
using exercise.minimalapi.Enums;
using exercise.minimalapi.Helpers;
using exercise.minimalapi.Models;
using exercise.minimalapi.Models.Payloads;
using exercise.minimalapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.minimalapi.Endpoints
{
    public static class BlogPostEndpoints
    {
        public static void ConfigurePostsEndpoints(this WebApplication app)
        {
            var posts = app.MapGroup("/posts");
            posts.MapGet("", GetPosts);
            posts.MapPost("", CreatePost);
            posts.MapPut("/{id}", EditPost);
        }

        [Authorize()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async static Task<IResult> GetPosts(IPostsRepo repository, ClaimsPrincipal user)
        {
            bool isAdmin = user.IsInRole("Admin");

            var posts = isAdmin  ? await repository.GetAll() : await repository.GetPostsByUserId(user.UserEmail());
            
            //var posts = await repository.GetAll();
            var PostsResult = new List<PostDTO>();
            if(posts.Count == 0) { return TypedResults.NoContent(); }

            foreach (var post in posts)
            {
                PostsResult.Add(new PostDTO(post));
            }
            return TypedResults.Ok(PostsResult);
        }

        [Authorize()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async static Task<IResult> CreatePost(IPostsRepo repository, PostPayload payload, ClaimsPrincipal user) 
        {
            var userId = user.UserId();
            if(userId == null) { return TypedResults.Unauthorized(); }

            if (payload.CheckPayload() != string.Empty)
            {
                return TypedResults.BadRequest(payload.CheckPayload());
            }

            var newPost = await repository.Add(payload.Text, user.Identity.Name);
            return TypedResults.Created($"/posts/{newPost.Id}", new PostDTO(newPost));
        }
        public static Task<IResult> EditPost() 
        {
         throw new NotImplementedException();
        }
    }
}
