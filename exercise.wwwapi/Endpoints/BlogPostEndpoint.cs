using System.Security.Claims;
using System.Xml.Serialization;
using AutoMapper;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoints(this WebApplication app)
        {
            app.MapGet("blogposts", GetBlogPosts);
            app.MapGet("blogposts/{id}", GetBlogPostById);
            app.MapPost("blogposts", CreateBlogPost);
            app.MapPut("blogposts/{id}", UpdateBlogPost);
            app.MapGet("blogposts/{id}/comments", GetComments);
            app.MapPost("blogposts/{id}/comments", CreateComment);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogPosts(IRepository<BlogPost> repo, IMapper mapper, ClaimsPrincipal user)
        {
            var blogPosts = repo.GetAll();
            var dto = mapper.Map<List<BlogPost>>(blogPosts);
            var payload = new Payload<List<BlogPost>>
            {
                Data = dto
            };
            return TypedResults.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogPostById(IRepository<BlogPost> repo, IMapper mapper, int id)
        {
            var blogPost = repo.GetById(id);
            var dto = mapper.Map<BlogPost>(blogPost);
            var payload = new Payload<BlogPost>
            {
                Data = dto
            };
            return TypedResults.Ok(payload);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateBlogPost(BlogPostRequestDTO request, IRepository<BlogPost> repository, IMapper mapper, ClaimsPrincipal user)
        {
            var authorId = user.UserId();
            var blogPost = new BlogPost
            {
                Text = request.Text,
                AuthorId = authorId
            };
            var result = await repository.Insert(blogPost);
            var dto = mapper.Map<BlogPost>(result);
            var payload = new Payload<BlogPost>
            {
                Status = "Blog Post Created Successfully",
                Data = dto
            };
            return TypedResults.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        private static async Task<IResult> UpdateBlogPost(BlogPostRequestDTO request, IRepository<BlogPost> repo, IMapper mapper, int id, ClaimsPrincipal user)
        {

            var authorId = user.UserId();
            if (authorId != null)
            {
                var blogPost = await repo.GetById(id);
                if (blogPost == null)
                {
                    return Results.NotFound();
                }
                if (blogPost.AuthorId != authorId)
                {
                    return Results.Unauthorized();
                }
                blogPost.Text = request.Text;
                var result = await repo.Update(blogPost);
                var dto = mapper.Map<BlogPost>(result);
                var payload = new Payload<BlogPost>
                {
                    Status = "Blog Post Updated Successfully",
                    Data = dto
                };
                return TypedResults.Ok(payload);
            }
            return Results.Unauthorized();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetComments(IRepository<Comment> repo, IMapper mapper, int id)
        {
            var comments = repo.GetAll(x => x.Where(c => c.BlogPostId == id));
            var dto = mapper.Map<List<Comment>>(comments);
            var payload = new Payload<List<Comment>>
            {
                Data = dto
            };
            return TypedResults.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateComment(CommentRequestDTO request, IRepository<Comment> repository, IMapper mapper, int id, ClaimsPrincipal user)
        {
            var commenterId = user.UserId();
            var comment = new Comment
            {
                Text = request.Text,
                BlogPostId = id,
                CommenterId = commenterId
            };
            var result = await repository.Insert(comment);
            var dto = mapper.Map<Comment>(result);
            var payload = new Payload<Comment>
            {
                Status = "Comment Created Successfully",
                Data = dto
            };
            return TypedResults.Ok(payload);
        }
    }
}
