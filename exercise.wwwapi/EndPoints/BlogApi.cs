
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.EndPoints
{
    public static class BlogApi
    {
        public static void ConfigureBlogEndpoint(this WebApplication app)
        {
            var blogGroup = app.MapGroup("posts");

            blogGroup.MapGet("/GetAll", GetAllPosts);
            blogGroup.MapPost("/CreatePost", CreateNewPost);
            blogGroup.MapPut("/editPost/{id}", EditPost);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllPosts(IDatabaseRepository<BlogPost> service)
        {
            var posts = service.GetAll();
            List<BlogResponse> result = new List<BlogResponse>();
            foreach (var post in posts)
            {
                BlogResponse dto = new BlogResponse();
                dto.user = post.User.Username;
                dto.PostId = post.Id;
                dto.PostText = post.BlogText;
                result.Add(dto);
            }
            var payload = new Payload<List<BlogResponse>>();    
            payload.data = result;


            return Results.Ok(payload);
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> CreateNewPost(IDatabaseRepository<BlogPost> service, BlogRequest request, ClaimsPrincipal claim)
        {

            // is user logged in?
            var userId = claim.UserRealId();
            if(userId == null)
            {
                return Results.Unauthorized();
            }

            var data = new BlogPost();
            data.BlogText = request.BlogText;
            data.UserId = (int)userId;

            service.Insert(data);
            service.Save();

            var post = service.GetAll().Where(x => x.UserId == data.UserId).FirstOrDefault();
            if(post == null)
            {
                return Results.NotFound();
            }

            var response = new BlogResponse();
            response.PostId = post.Id;
            response.PostText = post.BlogText;
            response.user = post.User.Username;
            
            var payload = new Payload<BlogResponse>();
            payload.data = response;

            return Results.Created($"http://localhost:7195/posts/{payload.data.PostId}", payload);
        }



        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> EditPost(IDatabaseRepository<BlogPost> service, int id, BlogRequest request, ClaimsPrincipal claim)
        {
            // is user logged in?
            var userId = claim.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //find post to edit
            var post = service.GetById(id);
            if (post == null)
            {
                return Results.Unauthorized();
            }

            if(userId != post.UserId)
            {
                return Results.Unauthorized();
            }

            post.BlogText = request.BlogText;
            service.Update(post);
            service.Save();

            var response = new BlogResponse();
            response.PostId = post.Id;
            response.PostText = post.BlogText;
            response.user = post.User.Username;


            var payload = new Payload<BlogResponse>();
            payload.data = response;

            return Results.Created($"http://localhost:7195/posts/{payload.data.PostId}", payload);
        }
    }
}
