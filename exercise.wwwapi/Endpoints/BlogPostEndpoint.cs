using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        private static string _path = AppContext.BaseDirectory;
        public static void BlogPostEndpointConfiguration(this WebApplication app)
        {
            var blogPosts = app.MapGroup("blogPosts");
            blogPosts.MapGet("/", GetBlogPosts);
            blogPosts.MapPost("/", CreateBlogPost);
            blogPosts.MapPut("/{id}", UpdateBlogPost);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBlogPosts(IRepository<BlogPost> repository)
        {
            var resultList = await repository.GetAll(inclusions: ["Course"]);
            var resultDTOs = new List<BlogPostResponseDTO>();
            foreach (var result in resultList)
            {
                resultDTOs.Add(new BlogPostResponseDTO(result));
            }
            var payload = new Payload<IEnumerable<BlogPostResponseDTO>>() { data = resultDTOs };
            return TypedResults.Ok(payload);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> CreateBlogPost(IRepository<BlogPost> repository, BlogPostPostDTO model)
        {
            var result = await repository.Create(
                inclusions: ["Author"],
                model: new BlogPost()
                {
                    Title = model.Title,
                    Content = model.Content,
                    UserId = model.AuthorId
                });

            var resultDTO = new BlogPostResponseDTO(result);

            var payload = new Payload<BlogPostResponseDTO>() { data = resultDTO };
            return TypedResults.Created(_path, payload);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> UpdateBlogPost(IRepository<BlogPost> repository, int id, BlogPostPutDTO model)
        {
            var result = await repository.Update(
                inclusions: ["Author"],
                new BlogPost()
                {
                    Title = model.Title,
                    Content = model.Content,
                    UserId = model.AuthorId,
                });

            var resultDTO = new BlogPostResponseDTO(result);

            var payload = new Payload<BlogPostResponseDTO>() { data = resultDTO };
            return TypedResults.Created(_path, payload);
        }
    }
}
