using AutoMapper;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Linq;
using System.Security.Claims;

namespace exercise.wwwapi.EndPoints
{
    public static class SecureApi
    {
        public static void ConfigureSecureApi(this WebApplication app)
        {
            var posts = app.MapGroup("/posts");
            posts.MapGet("/", GetAllBlogPosts);
            posts.MapPost("/", CreateBlogPost);
            posts.MapPut("/", EditBlogPost);
           

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllBlogPosts(IRepository<BlogPost> service, ClaimsPrincipal user, ILogger logger, IMapper mapper)
        {
            return Results.Ok(new Payload<List<BlogPostRequestDTO>>(mapper.Map<List<BlogPostRequestDTO>>(service.GetAll().ToList())));
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateBlogPost(IRepository<BlogPost> service, BlogPostRequestDTO payload, ClaimsPrincipal user, ILogger logger, IMapper mapper)
        {
            BlogPost createdBlogPost = new BlogPost()
            {
                Text = payload.Text,
                AuthorId = user.UserRealId().Value
            };
            service.Insert(createdBlogPost);
            service.Save();
            return Results.Ok(new Payload<BlogPost>(createdBlogPost));
        }

        private static async Task<IResult> EditBlogPost(IRepository<BlogPost> service, BlogPostRequestDTO payload, int id, ClaimsPrincipal user, ILogger logger, IMapper mapper)
        {
            BlogPost blogpost = service.GetById(id);
            if(blogpost == null) 
            {
                return Results.NotFound("BlogPost not found");
            }
            blogpost.Text = payload.Text;
            blogpost.AuthorId = user.UserRealId().Value;
            service.Update(blogpost);
            service.Save();
            return Results.Ok(new Payload<BlogPost>(mapper.Map<BlogPost>(blogpost)));
        }
    }
}
