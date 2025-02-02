using System.Security.Claims;
using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace exercise.wwwapi.EndPoints
{
    public static class BlogEndpoint
    {
        public static void ConfigureBlogApi(this WebApplication app)
        {
            app.MapGet("getBlogs", GetBlogs);
            app.MapGet("getBlogs/{id}", GetBlog);
            app.MapPost("AddBlogs", AddBlog);
            app.MapPut("updateBlog/{id}", UpdateBlog);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetBlogs(IRepository<Blog> repository, IMapper mapper)
        {
            var blogs = await repository.GetWithIncludes(p => p.User);

            if (blogs == null || !blogs.Any())
            {
                return TypedResults.NotFound(new Payload<string> { status = "error", data = "No blogs found." });
            }

            var response = mapper.Map<List<BlogDTO>>(blogs);
            return TypedResults.Ok(new Payload<List<BlogDTO>> { data = response });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetBlog(IRepository<Blog> repository, IMapper mapper, int id)
        {
            var blog = await repository.GetByIdWithIncludes(id, p => p.User);
            if (blog == null)
            {
                return TypedResults.NotFound(new Payload<string> { status = "error", data = $"Blog with ID {id} not found." });
            }

            var response = mapper.Map<BlogDTO>(blog);
            return TypedResults.Ok(new Payload<BlogDTO> { data = response });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> AddBlog(IRepository<Blog> repository, IMapper mapper, AddBlogDTO blogDto, ClaimsPrincipal user)
        {
            int? userId = user.UserRealId();
            if (userId == null)
            {
                return TypedResults.BadRequest(new Payload<string> { status = "error", data = "User ID could not be determined." });
            }

            Blog create = new Blog()
            {
                Title = blogDto.Title,
                Content = blogDto.Content,
                Description = blogDto.Description,
                Author = blogDto.Author,
                UserId = userId.Value
            };

            repository.Insert(create);
            repository.Save();

            var response = mapper.Map<AddBlogDTO>(create);
            return TypedResults.Created($"/blogs/{create.Id}", new Payload<AddBlogDTO> { data = response });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateBlog(IRepository<Blog> repository, IMapper mapper, int id, UpdateDTO blogDto, ClaimsPrincipal user)
        {
            var blog = repository.GetById(id);
            if (blog == null)
            {
                return TypedResults.NotFound(new Payload<string> { status = "error", data = "Blog not found." });
            }
            if (blog.UserId != user.UserRealId())
            {
                return TypedResults.BadRequest(new Payload<string> { status = "error", data = "Unauthorized to update this blog." });
            }

            blog.Title = blogDto.Title;
            blog.Content = blogDto.Content;
            blog.Description = blogDto.Description;
            blog.Author = blogDto.Author;

            repository.Update(blog);
            repository.Save();

            var response = mapper.Map<BlogDTO>(blog);
            return TypedResults.Ok(new Payload<BlogDTO> { data = response });
        }
    }
}
