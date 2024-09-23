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
            app.MapPut("edit/{id}", EditPost);
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
                        AuthorId = blogpost.AuthorId
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

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> EditPost(IDatabaseRepository<BlogPost> blogservice, ClaimsPrincipal user, BlogPUTModel model, int id)
        {
            var userId = user.UserRealId();
            if (userId != null)
            {
                var toChange = blogservice.GetById(id);
                if (model.Text != "")
                {
                    toChange.Text = model.Text;
                }
                if (model.AuthorId != 0)
                {
                    toChange.AuthorId = model.AuthorId;
                }
                blogservice.Update(toChange);
                blogservice.Save();
                BlogPostDTO posted = new BlogPostDTO()
                {
                    Text = toChange.Text,
                    AuthorId = toChange.AuthorId
                };
                return TypedResults.Created("success!", posted);
            }
            else
            {
                return TypedResults.Unauthorized();
            }
        }
    }
}
