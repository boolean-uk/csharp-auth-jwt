using wwwapi.Extensions;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoints
    {
        public static void ConfigureBlogEndpoints(this WebApplication app)
        {
            var usergroup = app.MapGroup("/blogposts");
            usergroup.MapPost("/", CreatePost);
            usergroup.MapPut("/{id}", EditPost);
            usergroup.MapGet("/", GetAllPosts);
        }

        [Authorize]
        private static async Task<IResult> EditPost(HttpContext context, IRepository<BlogPost> repo, Update_BlogPost dto, int id )
        {
            try
            {
                var updated = await dto.Update(repo, id);
                return TypedResults.Created(context.Get_endpointUrl<int>(id),Get_BlogPost.toPayload(updated));
            }
            catch (HttpRequestException ex)
            {
                return Fail.Payload(ex);
            }
        }

        [Authorize]
        private static async Task<IResult> GetAllPosts(HttpContext context, IRepository<BlogPost> repo)
        {
            
            return TypedResults.Ok(await Get_BlogPost.toPayload(repo));
        }

        [Authorize]
        private static async Task<IResult> CreatePost(HttpContext context, IRepository<BlogPost> repo, Create_BlogPost dto)
        {
            try
            {
                BlogPost post = await dto.Create(repo);
                return TypedResults.Created(context.Get_endpointUrl<int>(post.Id), Get_BlogPost.toPayload(post));
            }
            catch (HttpRequestException ex)
            {
                return Fail.Payload(ex);
            }

        }
    }
}
