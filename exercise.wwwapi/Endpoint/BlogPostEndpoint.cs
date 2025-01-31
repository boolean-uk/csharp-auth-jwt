using exercise.wwwapi.Helpers;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoint
{
    public static class BlogPostEndpoints
    {
        public static void ConfigureBlogPostEndpoints(this WebApplication app)
        {
            var posts = app.MapGroup("/api/posts");
            posts.MapGet("/", GetAllPosts);
            posts.MapPost("/", CreatePost).RequireAuthorization("AdminOrAuthor");
            posts.MapPut("/{id}", EditPost).RequireAuthorization("AdminOrAuthor");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAllPosts(IBlogPostRepository repository)
        {
            var posts = await repository.GetAllPostsAsync();
            return TypedResults.Ok(posts);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreatePost([FromServices] IBlogPostRepository repository, [FromBody] BlogPostDTO post, ClaimsPrincipal user)
        {
            var userId = user.UserRealId(); // Use your helper method instead of FindFirstValue
            if (!userId.HasValue)
                return TypedResults.Unauthorized();

            var userName = user.FindFirstValue(ClaimTypes.Name);

            // Map BlogPostDTO to BlogPost
            var newPost = new BlogPost
            {
                Text = post.Text,
                AuthorId = userId.ToString(),
                AuthorUsername = userName
            };

            var createdPost = await repository.CreatePostAsync(newPost); // Use BlogPost instead of BlogPostDTO

            return TypedResults.Created($"/api/posts/{createdPost.Id}", createdPost);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> EditPost(int id, [FromBody] BlogPostDTO post, ClaimsPrincipal user, [FromServices] IBlogPostRepository repository)
        {
            var userId = user.UserRealId(); // Use your helper method instead of FindFirstValue
            if (!userId.HasValue)
                return TypedResults.Unauthorized();

            var existingPost = await repository.GetPostByIdAsync(id);
            if (existingPost == null)
                return TypedResults.NotFound();

            // Ensure that the current user is either the author or an admin
            if (existingPost.AuthorId != userId.ToString() && user.FindFirstValue(ClaimTypes.Role) != "Admin")
                return TypedResults.Forbid();

            var userName = user.FindFirstValue(ClaimTypes.Name);

            // Map BlogPostDTO to BlogPost
            existingPost.Text = post.Text;

            var updatedPost = await repository.EditPostAsync(id, existingPost); // Pass the existingPost with blogpostdto values
            return TypedResults.Ok(updatedPost);
        }
    }
}
