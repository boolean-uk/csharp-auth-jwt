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


    }
}
