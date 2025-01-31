using AutoMapper;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;

namespace exercise.wwwapi.EndPoints
{
    public static class SecureApi
    {
        public static void ConfigureSecureApi(this WebApplication app)
        {
            app.MapGet("posts", GetPosts);
            app.MapPost("posts", AddPost);
            app.MapPut("posts{id}", UpdatePost);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<BlogPost> repository, ClaimsPrincipal user, IMapper mapper)
        {
            return Results.Ok(new Payload<List<BlogPostRequestDto>>(mapper.Map<List<BlogPostRequestDto>>(repository.GetAll().ToList())));
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> AddPost(IRepository<BlogPost> repository, BlogPostRequestDto newPost, ClaimsPrincipal user, IMapper mapper)
        {
            BlogPost blogPost = new BlogPost()
            {
                Text = newPost.Text,
                UserId = user.UserRealId().Value,
            };
            repository.Insert(blogPost);
            repository.Save();

            var response = mapper.Map<BlogPostResponseDto>(blogPost);

            return Results.Ok(new Payload<BlogPostResponseDto>(response));
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdatePost(IRepository<BlogPost> repository, BlogPostRequestDto newPost, int id, ClaimsPrincipal user, IMapper mapper)
        {
            BlogPost blogPost = repository.GetById(id);
            if (blogPost == null)
            {
                return Results.NotFound("Could not find blog post");
            }
            blogPost.Text = newPost.Text;
            blogPost.UserId = user.UserRealId().Value;
            repository.Update(blogPost);
            repository.Save();

            var response = mapper.Map<BlogPostResponseDto>(blogPost);

            return Results.Ok(new Payload<BlogPostResponseDto>(response));
        }
    }
}
