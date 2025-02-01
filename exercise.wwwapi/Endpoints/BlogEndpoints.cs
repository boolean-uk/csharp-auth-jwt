using wwwapi.Extensions;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using System.Security.Claims;
using System.Net;
using exercise.wwwapi.Payload;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoints
    {
        public static void ConfigureBlogEndpoints(this WebApplication app)
        {
            var usergroup = app.MapGroup("/blogposts");
            usergroup.MapPost("/", CreatePost);
            usergroup.MapPost("/{id}/comment", CreatePostComment);
            usergroup.MapPut("/{id}", EditPost);
            usergroup.MapGet("/", GetAllPosts);
            usergroup.MapGet("withcomments/", GetAllUsersPosts_withComments);
        }

        [Authorize]
        private static async Task<IResult> EditPost(HttpContext context, IRepository<BlogPost> repo, Update_BlogPost dto, ClaimsPrincipal user, int id )
        {
            try
            {
                var updated = await dto.Update(repo, user, id);
                return TypedResults.Created(context.Get_endpointUrl<int>(id),Get_BlogPost.toPayload(updated));
            }
            catch (HttpRequestException ex)
            {
                return Fail.Payload(ex);
            }
        }
        [Authorize]
        private static async Task<IResult> CreatePostComment(HttpContext context, IRepository<Comment> repo, Create_BlogPostComment dto, ClaimsPrincipal user, int id )
        {
            try
            {
                var updated = await dto.Create(repo, user, id);
                return TypedResults.Created(context.Get_endpointUrl<int>(id), Get_Comment.toPayload(updated));
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
        private static async Task<IResult> GetAllUsersPosts_withComments(HttpContext context, IRepository<BlogPost> repo)
        {

            return TypedResults.Ok(await Get_BlogPostWithComments.toPayload(repo));
        }

        [Authorize]
        private static async Task<IResult> CreatePost(HttpContext context, IRepository<BlogPost> repo, ClaimsPrincipal user, Create_BlogPost dto)
        {
            try
            {
                BlogPost post = await dto.Create(repo, user);
                return TypedResults.Created(context.Get_endpointUrl<int>(post.Id), Get_BlogPost.toPayload(post));
            }
            catch (HttpRequestException ex)
            {
                return Fail.Payload(ex);
            }

        }
    }
}
