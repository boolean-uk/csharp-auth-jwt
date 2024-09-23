using exercise.wwwapi.DTOs;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Payload;
using exercise.wwwapi.Repository;
using exercise.wwwapi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoint
    {
        public static void ConfigureBlogApi(this WebApplication app)
        {
            app.MapPost("create", CreatePost);
            app.MapGet("posts", GetPosts);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IDatabaseRepository<BlogPost> blogservice, BlogPostModel model, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId != null)
            {
                blogservice.Insert(new BlogPost()
                {
                    Text = model.Text,
                    AuthorId = (int) userId
                });
                blogservice.Save();
                BlogPostDTO posted = new BlogPostDTO()
                {
                    Text = model.Text,
                    AuthorId = (int) userId            
                };
                return TypedResults.Created("success!", posted);
            }
            else
            {
                return Results.Unauthorized();
            }

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IDatabaseRepository<BlogPost> blogservice, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId != null)
            {
                var found = blogservice.GetAll();
                Payload<List<BlogAllDTO>> payload = new Payload<List<BlogAllDTO>>();
                payload.data = new List<BlogAllDTO>();

                foreach (var blogpost in found)
                {
                    payload.data.Add(new BlogAllDTO()
                    {
                        Id = blogpost.Id,
                        Text = blogpost.Text,
                        AuthorId = (int) userId
                    });
                }
                payload.status = "success";
                return TypedResults.Ok(payload);
            }
            else
            {
                return Results.Unauthorized();
            }
        }
    }
}
