using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Request;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var blog = app.MapGroup("BlogPosts");

            blog.MapPost("/", AddBlogPost);
            blog.MapGet("/", GetAll);
            blog.MapGet("/{id}", GetById);
            blog.MapPut("/{id}", Update);

        }

        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> AddBlogPost(IRepository<BlogPost> repository, BlogpostPost blogPost)
        {
            var results = await repository.Get();

            var entity = new BlogPost
            {
                text = blogPost.Text,
                AuthourId = Guid.NewGuid().ToString()
            };
            await repository.CreateBlogPost(entity);
            return TypedResults.Created($"/{entity.Id}", new { Text = blogPost.Text, AuthorId = entity.AuthourId });

        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAll(IRepository<BlogPost> repository)
        {
            var entities = await repository.Get();
            List<Object> results = new List<Object>();
            foreach (var blogPost in entities)
            {
                results.Add(new { Id = blogPost.Id, Text = blogPost.text, AuthorId = blogPost.AuthourId });
            }
            return TypedResults.Ok(results);
        }



        [Authorize(Roles = "User, Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetById(IRepository<BlogPost> repository, int id)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find any posts with authorid:{id}");
            }
            return TypedResults.Ok(new { Text = entity.text, AuthorId = entity.AuthourId });
        }




        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> Update(IRepository<BlogPost> repository, int id, BlogPostUpdate request)
        {
            var entity = await repository.GetById(id);
            if (entity == null)
            {
                return TypedResults.NotFound($"Could not find post with Id:{id}");
            }
            entity.text = !string.IsNullOrEmpty(request.Text) ? request.Text : entity.text;

            var result = await repository.Update(entity);

            return result != null ? TypedResults.Ok(new { AuthorId = result.AuthourId, Text = result.text }) : TypedResults.BadRequest("Couldn't save to the database?!");
        }
    }
}
