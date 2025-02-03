using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query;
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
            var postsDTO = posts.Select(p => new BlogGetAllDTO

            {
                Id = p.Id,
                Text = p.Text,
                AuthorId = p.AuthorId,
                UserName = p.UserName,
            });
            var response = new Payload<IEnumerable<BlogGetAllDTO>> { data = postsDTO };

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
             
            var mapster = post.Adapt<BlogResponseDTO>();


            var response = new Payload<BlogResponseDTO> { data = mapster };

            return TypedResults.Ok(response);

        } 

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateBlogPost(IRepository<BlogPost> repository, BlogPostDTO blogPostDTO, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            var userName = user.FindFirstValue(ClaimTypes.Name);

            var post = new BlogPost
            {
                Text = blogPostDTO.Text,
                AuthorId = userId.ToString(),
                UserName = userName

            };



            var createdPost = repository.Add(post);

            var response = createdPost.Adapt<BlogResponseDTO>();

            return Results.Ok(response);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdateBlogPost(IRepository<BlogPost> repository, int id, BlogPostUpdateDTO blogPostUpdateDTO, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            var userName = user.FindFirstValue(ClaimTypes.Name);

            Console.WriteLine("HHHHHER:"+userId);

            if (userId == null)
            {
                Console.WriteLine("HHHHHER:" + userId);
                return Results.Unauthorized();

            }

            var postToUpdate = repository.GetById(id);


            if (postToUpdate.AuthorId == userId.ToString())
            {

                postToUpdate.Text = blogPostUpdateDTO.Text;
                postToUpdate.AuthorId = blogPostUpdateDTO.AuthorId;
                postToUpdate.UserName = blogPostUpdateDTO.UserName;

                
                repository.Update(postToUpdate);
                repository.Save();

                var response = new BlogPostUpdateDTO
                { 
                    Text = postToUpdate.Text,
                    AuthorId = postToUpdate.AuthorId,
                    UserName = postToUpdate.UserName,
                
                };

                

                return TypedResults.Ok(response);
            }

            return Results.Unauthorized();

        }
    }

}