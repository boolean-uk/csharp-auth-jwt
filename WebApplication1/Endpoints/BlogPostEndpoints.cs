using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net.Sockets;
using WebApplication1.Repository;
using WebApplication1.Models.DTO;

using static WebApplication1.Models.BlogPost;

namespace WebApplication1.Endpoints
{
    public static class BlogPostEndpoints
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var blogGroup = app.MapGroup("blogposts");

            blogGroup.MapGet("/blogposts/", GetBlogPosts);

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBlogPosts(Irepository repository)
        {
            var blogPosts = await repository.GetBlogPosts();
            List<BlogPostResponseDTO> blogPostsToReturn = new List<BlogPostResponseDTO>();
            
            foreach (var blogPost in blogPosts)
            {
                BlogPostResponseDTO blog = new BlogPostResponseDTO(blogPost);

                blogPostsToReturn.Add(blog);
            }

            return TypedResults.Ok(blogPostsToReturn);
        }
    }
}
