using csharp_auth_jwt.Model;
using csharp_auth_jwt.Model.Dto;
using csharp_auth_jwt.Model.Enum;
using csharp_auth_jwt.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace csharp_auth_jwt.Endpoints
{
    public static class BlogPostEndpoints
    {
        public static void ConfigureBlogPostEndpoints(this WebApplication app)
        {
            var blogPostGroup = app.MapGroup("posts");
            blogPostGroup.MapGet("/" , GetAllBlogPosts).RequireAuthorization();
            blogPostGroup.MapPost("/" , CreateBlogPost).RequireAuthorization();
            blogPostGroup.MapPut("/{id}" , UpdateBlogPost).RequireAuthorization();
            blogPostGroup.MapDelete("/{id}" , DeleteBlogPost).RequireAuthorization();
        }

        public static async Task<IActionResult> GetAllBlogPosts([FromServices] IBlogPostRepository repository)
        {
            var blogPosts = await repository.GetBlogPosts();
            return (IActionResult)Results.Ok(blogPosts.Select(bp => new BlogPostDto { Id = bp.Id , Text = bp.Text , AuthorId = bp.AuthorId }));
        }

        public static async Task<IActionResult> CreateBlogPost(BlogPostDto blogPostDto , [FromServices] IBlogPostRepository repository , [FromServices] UserManager<BlogUser> userManager)
        {
            var user = await userManager.GetUserAsync(User);
            if(user == null)
            {
                return Unauthorized();
            }

            var blogPost = new BlogPost { Text = blogPostDto.Text , AuthorId = user.Id };
            await repository.AddBlogPost(blogPost);
            return Created($"/posts/{blogPost.Id}" , blogPost);
        }

        public static async Task<IActionResult> UpdateBlogPost(int id , BlogPostDto blogPostDto , [FromServices] IBlogPostRepository repository , [FromServices] UserManager<BlogUser> userManager)
        {
            var blogPost = await repository.GetBlogPost(id);
            if(blogPost == null)
            {
                return NotFound();
            }

            var user = await userManager.GetUserAsync(User);
            if(user == null || (user.Id != blogPost.AuthorId && user.Role != BlogUserRole.Admin))
            {
                return Forbid();
            }

            blogPost.Text = blogPostDto.Text;
            await repository.UpdateBlogPost(blogPost);
            return NoContent();
        }

        public static async Task<IActionResult> DeleteBlogPost(int id , [FromServices] IBlogPostRepository repository , [FromServices] UserManager<BlogUser> userManager)
        {
            var blogPost = await repository.GetBlogPost(id);
            if(blogPost == null)
            {
                return NotFoundResult();
            }

            var user = await userManager.GetUserAsync(User);
            if(user == null || (user.Id != blogPost.AuthorId && user.Role != BlogUserRole.Admin))
            {
                return Forbid();
            }

            await repository.DeleteBlogPost(id);
            return NoContent();
        }
    }

}
