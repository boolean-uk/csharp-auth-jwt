using AutoMapper;
using exercise.wwwapi.Models.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using exercise.wwwapi.Helpers;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        private static string _path = AppContext.BaseDirectory;
        public static void ConfigureBlogPostEndpoints(this WebApplication app)
        {
            var blogPosts = app.MapGroup("posts");
            blogPosts.MapGet("/", GetBlogPosts);
            blogPosts.MapPost("/", CreateBlogPost);
            blogPosts.MapPut("/{id}", UpdateBlogPost);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBlogPosts(IRepository<BlogPost> repository, IMapper mapper, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var resultList = await repository.GetAll();

            var resultDTOs = mapper.Map<IEnumerable<GetBlogPostDTO>>(resultList);

            var payload = new Payload<IEnumerable<GetBlogPostDTO>>() { Data = resultDTOs };

            return TypedResults.Ok(payload);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateBlogPost(IRepository<BlogPost> repository, PostBlogPostDTO model, ClaimsPrincipal user, IMapper mapper) 
        {
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var blogPost = mapper.Map<BlogPost>(model);
            blogPost.AuthorId = (int)userId; 

            var createdPost = await repository.Create(blogPost);

            var responseDTO = mapper.Map<GetBlogPostDTO>(createdPost);

            var payload = new Payload<GetBlogPostDTO>() { Data = responseDTO };

            return TypedResults.Created(_path, payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateBlogPost( IRepository<BlogPost> repository, int id, PutBlogPostDTO model, ClaimsPrincipal user,IMapper mapper)
        {
            
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            
            var existingPost = await repository.Get(x => x.Id == id);
            if (existingPost == null)
            {
                return Results.NotFound(new Payload<string> { Status = "Blog post not found" });
            }

            
            mapper.Map(model, existingPost);
            existingPost.AuthorId = userId.Value; 

            await repository.Update(existingPost);

            var responseDTO = mapper.Map<GetBlogPostDTO>(existingPost);

            var payload = new Payload<GetBlogPostDTO>() { Data = responseDTO };
            return TypedResults.Ok(payload); 
        }


    }
}
