using AutoMapper;
using exercise.wwwapi.DTO.Requests;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class SecureApi
    {
        public static void ConfigureSecureApi(this WebApplication app)
        {
            var blog = app.MapGroup("/blog");
            blog.MapGet("/", GetBlogs);
            blog.MapPost("/", CreateBlog);
            blog.MapPut("/{id}", UpdateBlog);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogs(IRepository<Blog> service, ClaimsPrincipal user, ILogger logger, IMapper mapper)
        {
            var blogs = service.GetAll(b => b.User);

            var result = blogs.Select(b => new BlogDTO
            {
                Header = b.Header,
                Text = b.Text,
                Username = b.User.Username
            }).ToList();
            return TypedResults.Ok(new Payload<List<BlogDTO>>{ data = result });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateBlog(IRepository<Blog> service, BlogPost model, ClaimsPrincipal user, ILogger logger, IMapper mapper)
        {
            Blog blog = new Blog()
            {
                Header = model.Header,
                Text = model.Text,
                UserId = user.UserRealId().Value
            };
            service.Insert(blog);
            service.Save();

            var result = new BlogDTO
            {
                Header = blog.Header,
                Text = blog.Text,
                Username = blog.User.Username
            };
            return Results.Created($"/blogs/{blog.Id}", new Payload<BlogDTO> { data = result });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdateBlog(IRepository<Blog> service, int id, BlogPut model, ClaimsPrincipal user, ILogger logger, IMapper mapper)
        {
            Blog blog = service.GetById(id);
            if (blog == null) return Results.NotFound("Screening not found");
            if (model.Header != null) blog.Header = model.Header;
            if (model.Text != null) blog.Text = model.Text;

            service.Update(blog);
            service.Save();

            var result = new BlogDTO
            {
                Header = blog.Header,
                Text = blog.Text,
                Username = blog.User.Username
            };
            return Results.Created($"/blogs/{id}", new Payload<BlogDTO> { data = result });
        }
    }
}
