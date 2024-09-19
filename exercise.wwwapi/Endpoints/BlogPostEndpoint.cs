using exercise.wwwapi.DTOs;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        private static string _path = AppContext.BaseDirectory;
        public static void BlogPostEndpointConfiguration(this WebApplication app)
        {
            var blogPosts = app.MapGroup("posts");
            blogPosts.MapGet("/", GetBlogPosts);
            blogPosts.MapPost("/", CreateBlogPost);
            blogPosts.MapPut("/{id}", UpdateBlogPost);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBlogPosts(IRepository<BlogPost> repository, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var resultList = await repository.GetAll();
            var resultDTOs = new List<BlogPostResponseDTO>();
            foreach (var result in resultList)
            {
                resultDTOs.Add(new BlogPostResponseDTO(result));
            }
            var payload = new Payload<IEnumerable<BlogPostResponseDTO>>() { data = resultDTOs };
            return TypedResults.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> CreateBlogPost(IRepository<BlogPost> repository, BlogPostPostDTO model, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await repository.Create(
                model: new BlogPost()
                {
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = (int) userId
                });

            BlogPost newBlogPost = await repository.Get(x => x.Id == result.Id);

            var resultDTO = new BlogPostResponseDTO(newBlogPost);

            var payload = new Payload<BlogPostResponseDTO>() { data = resultDTO };
            return TypedResults.Created(_path, payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> UpdateBlogPost(IRepository<BlogPost> repository, int id, BlogPostPutDTO model, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var result = await repository.Update(
                new BlogPost()
                {
                    Id = id,
                    Title = model.Title,
                    Content = model.Content,
                    AuthorId = model.AuthorId
                });

            BlogPost updatedBlogPost = await repository.Get(x => x.Id == result.Id);

            var resultDTO = new BlogPostResponseDTO(updatedBlogPost);

            var payload = new Payload<BlogPostResponseDTO>() { data = resultDTO };
            return TypedResults.Created(_path, payload);
        }
    }
}
