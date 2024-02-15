using AutoMapper;
using exercise.Application;
using exercise.Data.Models;
using exercise.Infrastructure;
using exercise.wwwapi.DTOs;
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
        }

        private static string GetUserId(IHttpContextAccessor contextAccessor) => contextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name)!;

        [Authorize]
        public static async Task<IResult> GetAll(IRepository<Post> repository, 
            IHttpContextAccessor contextAccessor,
            IMapper mapper)
        {
            ServiceResponse<List<GetPostDTO>> response = new();
            try
            {
                List<Post> posts = await repository.GetAll();
                response.Data = posts.Where(p => p.UserId == GetUserId(contextAccessor))
                    .Select(mapper.Map<GetPostDTO>)
                    .ToList();
                return TypedResults.Ok(response);
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return TypedResults.BadRequest(response);
            }
        }

        [Authorize]
        public static async Task<IResult> Add(IRepository<Post> repository,
            IHttpContextAccessor contextAccessor,
            IMapper mapper,
            AddPostDTO addPostDTO)
        {
            ServiceResponse<GetPostDTO> response = new();
            try
            {
                Post post = mapper.Map<Post>(addPostDTO);
                post.UserId = GetUserId(contextAccessor);
                // post.PostedAt = DateTime.Now;
                post = await repository.Add(post);
                response.Data = mapper.Map<GetPostDTO>(post);
                return TypedResults.Created(nameof(Add), response);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return TypedResults.BadRequest(response);
            }
        }

        //[Authorize]
        //public static async Task<IResult> Delete(IRepository<Post> repository,
        //    IHttpContextAccessor,
        //    IMapper mapper,
        //    AddPost)
    }
}
