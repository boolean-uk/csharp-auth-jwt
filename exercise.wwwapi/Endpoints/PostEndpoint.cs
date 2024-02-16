using AutoMapper;
using exercise.Application;
using exercise.Data.Models;
using exercise.Infrastructure;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class PostEndpoint
    {
        public static void ConfigurePostEndpoint(this WebApplication app)
        {
            var group = app.MapGroup("/post");
            group.MapGet("/", GetAll);
            group.MapPost("/", Add);
            group.MapPut("/{postId}", Change);
        }

        private static string GetUserId(IHttpContextAccessor contextAccessor) => contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

        [Authorize]
        public static async Task<IResult> GetAll(PostService postService, IHttpContextAccessor contextAccessor)
        {
            var response = await postService.GetAllPostsByUser(GetUserId(contextAccessor));
            if (!response.Success)
            {
                return TypedResults.BadRequest(response);
            }
            return TypedResults.Ok(response);
        }

        [Authorize]
        public static async Task<IResult> Add(PostService postService,
            IHttpContextAccessor contextAccessor,
            AddPostDTO addPostDTO)
        {
            var response = await postService.AddPost(GetUserId(contextAccessor), addPostDTO);
            if (!response.Success)
            {
                return TypedResults.BadRequest(response);
            }
            return TypedResults.Created(nameof(Add), response);
        }

        [Authorize]
        public static async Task<IResult> Change(PostService postService,
            IHttpContextAccessor contextAccessor,
            AddPostDTO addPostDTO,
            string postId)
        {
            var response = await postService.ChangePost(GetUserId(contextAccessor), addPostDTO, postId);
            if (!response.Success)
            {
                return TypedResults.BadRequest(response);
            }
            return TypedResults.Ok(response);
        }
    }
}
