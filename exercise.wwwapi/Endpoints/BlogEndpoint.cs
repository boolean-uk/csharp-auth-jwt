using exercise.wwwapi.DataModels.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoint 
    {
        public static void ConfigureBlogEndpoint(this WebApplication app)
        {
            var blogs = app.MapGroup("blogs");
            blogs.MapGet("/", GetAllBlogs);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetAllBlogs(IRepository<BlogPost> repository)
        {
            var entities = await repository.SelectAll();
            if (entities == null)
            {
                return Results.NotFound("No blogs are found");
            }
            return Results.Ok(entities);
        }
    }
}
