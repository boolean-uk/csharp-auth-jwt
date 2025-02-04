using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;

namespace exercise.wwwapi.EndPoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            app.MapGet("blogpost", GetAll);
            app.MapGet("blogpost/{id}", GetById);
            app.MapPost("blogpost", AddBlogPost);
            app.MapPut("blogpost/{id}", UpdateBlogPost);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAll(IRepository<BlogPost> repository, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();

            if (userId != null)
            {
                var blogposts = repository.GetAll();
                Payload<List<BlogPostLinkDTO>> payload = new Payload<List<BlogPostLinkDTO>>();
                payload.data = new List<BlogPostLinkDTO>();

                foreach (var blogpost in blogposts)
                {
                    payload.data.Add(new BlogPostLinkDTO()
                    {
                        id = blogpost.id,
                        text = blogpost.text,
                        authorId = blogpost.authorId
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetById(IRepository<BlogPost> repo, int id)
        {
            return TypedResults.Ok(repo.GetById(id));
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> AddBlogPost(BlogPostDTO request, IRepository<BlogPost> repository, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId != null)
            {
                var blogpost = new BlogPost()
                {
                    text = request.Text,
                    authorId = userId.Value
                };
                repository.Insert(blogpost);
                repository.Save();
                return Results.Ok(new Payload<string>() { data = "Created BlogPost" });
            }
            else
            {
                return Results.Unauthorized();
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        private static async Task<IResult> UpdateBlogPost(BlogPostDTO request, IRepository<BlogPost> repository, int id, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId != null)
            {
                var blogpost = repository.GetById(id);
                if (blogpost == null)
                {
                    return Results.NotFound();
                }
                if (blogpost.authorId != userId.Value)
                {
                    return Results.Unauthorized();
                }
                blogpost.text = request.Text;
                repository.Update(blogpost);
                repository.Save();
                return Results.Ok(new Payload<string>() { data = "Updated BlogPost" });
            }
            else
            {
                return Results.Unauthorized();
            }
        }
    }
}
