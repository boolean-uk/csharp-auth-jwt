using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Mvc;
using exercise.wwwapi.Models;
using static exercise.wwwapi.DTOs.DTOs;
using static exercise.wwwapi.DTOs.payloads;
using Microsoft.AspNetCore.Authorization;

namespace exercise.wwwapi.Endpoints
{
    public static class ExerciseApi
    {
        public static void ConfigurePostsApi(this WebApplication app)
        {
            app.MapGet("/posts", GetPosts);
            app.MapPost("/posts/", CreatePost);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()] //you can not call get all posts unless logged in
        public static async Task<IResult> GetPosts(IRepository repository)
        {
            var posts = await repository.GetPostsAsync();
            var postDto = new List<PostResponseDTO>();
            foreach (Blogpost post in posts)
            {
                postDto.Add(new PostResponseDTO(post));
            }
            return TypedResults.Ok(postDto);
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreatePost(CreatePostPayload payload, IRepository repository)
        {
            if (payload.Title == null || payload.Title == "" || payload.Text == "" || payload.AuthorID == null)
            {
                return Results.BadRequest("A non-empty Title and/or text, authorID is required");
            }

            Blogpost? post = await repository.CreatePost(payload.Title, payload.Text, payload.AuthorID);
            if (post == null)
            {
                return Results.BadRequest("Failed to create post.");
            }

            return TypedResults.Created($"/posts/{post.Id}", new PostResponseDTO(post));
        }

    }
}
