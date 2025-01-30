using System.Security.Claims;
using AutoMapper;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostApi
    {
        public static void ConfigureBlogPostApi(this WebApplication app)
        {
            var posts = app.MapGroup("/posts");

            posts.MapGet("/", GetAllPosts);
            posts.MapPost("/", CreatePost);
            posts.MapPut("/{id}", UpdatePost);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllPosts(IRepository<BlogPost> service, IMapper mapper)
        {
            var posts = await service.GetWithIncludes(p => p.User);
            var dtos = mapper.Map<IEnumerable<BlogPostResponseDto>>(posts);
            var response = new Payload<IEnumerable<BlogPostResponseDto>>() { data = dtos };

            return TypedResults.Ok(response);
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IRepository<BlogPost> service, IMapper mapper, BlogPostRequestDto blogRequest, ClaimsPrincipal claim)
        {
            var post = mapper.Map<BlogPost>(blogRequest);
            if (claim.UserRealId() != null)
            {
                post.UserId = (int)claim.UserRealId();
                service.Insert(post);
                service.Save();

                var postWithUser = await service.GetByIdWithIncludes(post.Id, p => p.User);
                var postResponse = mapper.Map<BlogPostResponseDto>(postWithUser);
                postResponse.UserName = postWithUser.User.Username;

                return TypedResults.Ok(new Payload<BlogPostResponseDto>() { data = postResponse });
            }
            return TypedResults.Unauthorized();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdatePost(IRepository<BlogPost> service, IMapper mapper, BlogPostRequestDto blogRequest, int id, ClaimsPrincipal claim)
        {
            var post = await service.GetByIdWithIncludes(id, p => p.User);
            if (post == null) return TypedResults.BadRequest(new Payload<string>() { data = "Post not found" });
            var userId = claim.UserRealId();
            if (userId != post.UserId || claim.Role() == "Admin") return TypedResults.Unauthorized();
            post.Text = blogRequest.Text;
            service.Update(post);
            service.Save();
            var postResponse = mapper.Map<BlogPostResponseDto>(post);
            return TypedResults.Ok(new Payload<BlogPostResponseDto>() { data = postResponse });

        }
    }
}
