using AutoMapper;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;

namespace exercise.wwwapi.EndPoints
{
    public static class BlogApi
    {
        public static void CounfigureBlogApi(this WebApplication app)
        {
            var blog = app.MapGroup("blog");
            blog.MapGet("/", GetBlogs);
            blog.MapPost("/", WriteBlog);
            blog.MapPut("/{id}", UpdateBlog);

        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogs(IRepository<Blog> service, IMapper mapper, ClaimsPrincipal user)
        {
            var blogs = mapper.Map<List<BlogResponseDTO>>(service.GetAll());
            return TypedResults.Ok(new PayloadCollection<BlogResponseDTO>() { Data = blogs });
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> WriteBlog(IRepository<Blog> service, IMapper mapper, BlogPOST model, ClaimsPrincipal user)
        {

            try
            {
                var blog = mapper.Map<Blog>(model);
                blog.UserId = (int)user.UserRealId();

                service.Insert(blog);
                service.Save();

                var response = mapper.Map<BlogResponseDTO>(blog);
                response.Authour = user.UserName();

                return TypedResults.Ok(new Payload<BlogResponseDTO>() { Status = "Successfully created", Data = response});
            }
            catch (Exception ex) { return TypedResults.BadRequest("Oops! Something went wrong: " + ex); }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdateBlog(IRepository<Blog> service, IMapper mapper, int id, BlogPUT model, ClaimsPrincipal user)
        {
            try
            {
                var exisitngBlog = await service.GetById(id);
                if (exisitngBlog.UserId != (int)user.UserRealId()) return TypedResults.Unauthorized();

                var blog = mapper.Map(model, exisitngBlog);

                service.Update(blog);
                service.Save();

                var response = mapper.Map<BlogResponseDTO>(blog);
                response.Authour = user.UserName();

                return TypedResults.Ok(new Payload<BlogResponseDTO>() { Status = "Successfully updated", Data = response });
            }
            catch (Exception ex) { return TypedResults.BadRequest("Oops! Something went wrong: " + ex); }
        }
    }
}
