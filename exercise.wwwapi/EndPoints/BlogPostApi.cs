using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class PostEndpoint
    {
        public static void ConfigureBlogPostApi(this WebApplication app)
        {
            var posts = app.MapGroup("posts");
            posts.MapGet("/", GetAllBlogPosts);
            posts.MapGet("/{id}", GetBlogPostById);
            posts.MapPost("/", CreateBlogPost);
            posts.MapPut("/{id}", UpdateBlogPost);

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAllBlogPosts(IRepository<BlogPost> repository)
        {

           
            var posts = repository.GetAll();
            var postsDTO = posts.Select(p => new BlogResponseDTO

            {
                Id = p.Id,
                Text = p.Text,
                AuthorId = p.AuthorId

            });
            var response = new Payload<IEnumerable<BlogResponseDTO>> { data = postsDTO };

            return Results.Ok(response);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogPostById(IRepository<BlogPost> repository, int id)
        {
            var post = repository.GetById(id);

            if (post == null)
                return TypedResults.NotFound();


            var postDTO = new BlogResponseDTO
            {
                Id = post.Id,
                Text = post.Text,
                AuthorId = post.AuthorId,

            };

            var response = new Payload<BlogResponseDTO> { data = postDTO };

            return TypedResults.Ok(response);

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateBlogPost(IRepository<BlogPost> repository, BlogPostDTO blogPostDTO)
        {
           
            BlogPost blogpost = new BlogPost
            {
                Id = blogPostDTO.Id,
                Text = blogPostDTO.Text,
                AuthorId = blogPostDTO.AuthorId

            };

            var createdPost = repository.Add(blogpost);

            return Results.Ok(createdPost);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdateBlogPost(IRepository<BlogPost> repository, int id, BlogPostUpdateDTO blogPostUpdateDTO, ClaimsPrincipal user)
        {
            var userId = user.UserId();

            if (userId == null)
            {
                return Results.Unauthorized();

            }

            var postToUpdate = repository.GetById(id);


            if (postToUpdate.AuthorId == userId)
            {

                postToUpdate.Text = blogPostUpdateDTO.Text;
                postToUpdate.AuthorId = blogPostUpdateDTO.AuthorId;

                var response = new BlogPostUpdateDTO
                { 
                    Text = postToUpdate.Text,
                    AuthorId = postToUpdate.AuthorId,
                
                };
                return TypedResults.Ok(response);
            }

            return Results.Unauthorized();

        }
    }

}