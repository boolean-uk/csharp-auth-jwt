using exercise.wwwapi.DTOs;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.EndPoints
{
    public static class BlogPostEndpoints
    {
        public static void ConfigureBlogPointsApi(this WebApplication app)
        {
            var posts = app.MapGroup("/posts");
            app.MapGet("", GetPosts);
            app.MapPost("", CreatePost);
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        
        private static async Task<IResult> GetPosts(IDatabaseRepository<BlogPost> service, ClaimsPrincipal user)
        {
            try
            {
                var results = await service.GetAll();

                List<GetBlogPostDTO> posts = new List<GetBlogPostDTO>();

                foreach(var post in results)
                {
                    GetBlogPostDTO blogPost = new GetBlogPostDTO()
                    {
                        Title = post.Title,
                        Text = post.Text,
                        AuthorName = post.User.Name
                    };
                    posts.Add(blogPost);
                }

                return TypedResults.Ok(posts);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        private static async Task<IResult> CreatePost(IDatabaseRepository<BlogPost> service, ClaimsPrincipal user, NewBlogPostDto newPost)
        {
            try
            {
                BlogPost post = new BlogPost()
                {
                    Title = newPost.Title,
                    Text = newPost.Text,
                    UserId = user.UserId(), //not sure abt this one
                    //User = this??????
                    BlogComments = new List<BlogComment>(),
                    Posted = DateTime.UtcNow
                };

                await service.Insert(post);
                await service.Save();
                return TypedResults.Ok(post);

            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }

        }


    }
}
