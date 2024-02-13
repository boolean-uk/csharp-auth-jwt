using csharp_auth_jwt.Model;
using csharp_auth_jwt.Model.Dto;
using csharp_auth_jwt.Model.Enum;
using csharp_auth_jwt.Repository;
using Microsoft.AspNetCore.Identity;

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

        public static async Task<IResult> GetAllBlogPosts(IBlogPostRepository repository)
        {
            var blogPosts = await repository.GetBlogPosts();
            return Results.Ok(blogPosts.Select(bp => new BlogPostDto { Id = bp.Id , Text = bp.Text , AuthorId = bp.AuthorId }));
        }

        public static async Task<IResult> CreateBlogPost(BlogPostDto blogPostDto , IBlogPostRepository repository , UserManager<BlogUser> userManager)
        {
            var user = await userManager.FindByIdAsync(blogPostDto.AuthorId);
            if(user == null || blogPostDto.Text == null)
            {
                return Results.BadRequest("You must fill all fields");
            }

            var blogPost = new BlogPost { Text = blogPostDto.Text , AuthorId = user.Id };
            var createdBlogPost = await repository.AddBlogPost(blogPost);
            if(createdBlogPost == null)
            {
                return Results.NotFound("Could not create post");
            }
            return Results.Created($"/posts/{createdBlogPost.Id}" , createdBlogPost);
        }

        public static async Task<IResult> UpdateBlogPost(int id , BlogPostDto blogPostDto , IBlogPostRepository repository , UserManager<BlogUser> userManager)
        {
            var blogPost = await repository.GetBlogPost(id);
            if(blogPost == null)
            {
                return Results.NotFound();
            }

            var user = await userManager.FindByIdAsync(blogPostDto.AuthorId);
            if(user == null || (user.Id != blogPost.AuthorId && user.Role != BlogUserRole.Admin))
            {
                return Results.Forbid();
            }

            blogPost.Text = blogPostDto.Text;
            var updatedBlogPost = await repository.UpdateBlogPost(blogPost);
            if(updatedBlogPost == null)
            {
                return Results.NotFound("Could not update post");
            }
            return Results.NoContent();
        }

        public static async Task<IResult> DeleteBlogPost(int id , IBlogPostRepository repository , UserManager<BlogUser> userManager)
        {
            var blogPost = await repository.GetBlogPost(id);
            if(blogPost == null)
            {
                return Results.NotFound();
            }

            var user = await userManager.FindByIdAsync(blogPost.AuthorId);
            if(user == null || (user.Id != blogPost.AuthorId && user.Role != BlogUserRole.Admin))
            {
                return Results.Forbid();
            }

            await repository.DeleteBlogPost(id);
            return Results.NoContent();
        }
    }
}
