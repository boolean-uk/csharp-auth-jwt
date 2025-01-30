using System.Security.Claims;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            app.MapGet("blogposts", GetBlogPosts);
            app.MapGet("blogposts/{id}", GetBlogPostById);
            app.MapPost("blogposts", CreateBlogPost);
            app.MapPut("blogposts/{id}", UpdateBlogPost);
            
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogPosts(IRepository<BlogPost> repository, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();

            if (userId != null)
            {
                var blogposts = repository.GetAll();
                Payload<List<BlogListDTO>> payload = new Payload<List<BlogListDTO>>();
                payload.data = new List<BlogListDTO>();

                foreach (var blogpost in blogposts)
                {
                    payload.data.Add(new BlogListDTO()
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogPostById(IRepository<BlogPost> repo, int id)
        {
            return TypedResults.Ok(repo.GetById(id));
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateBlogPost(BlogPostRequestDTO request, IRepository<BlogPost> repository, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId != null)
            {
                var blogpost = new BlogPost()
                {
                    Text = request.Text,
                    AuthorId = userId.Value
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

        private static async Task<IResult> UpdateBlogPost(BlogPostRequestDTO request, IRepository<BlogPost> repository, int id, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId != null)
            {
                var blogpost = repository.GetById(id);
                if (blogpost == null)
                {
                    return Results.NotFound();
                }
                if (blogpost.AuthorId != userId.Value)
                {
                    return Results.Unauthorized();
                }
                blogpost.Text = request.Text;
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
