using System.Security.Claims;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoint
{
    public static class CommentEndpoints
    {
        public static void ConfigureCommentEndpoints(this WebApplication app)
        {
            var comments = app.MapGroup("/api/comments");
            comments.MapPost("/add/{postId}", AddCommentToPost).RequireAuthorization("AdminOrAuthor");
            comments.MapGet("/post/{postId}", GetCommentsForPost);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> AddCommentToPost(int postId, [FromBody] Comment comment, [FromServices] IBlogPostRepository repository, ClaimsPrincipal user)
        {
            // Ensure the user is authenticated
            var userId = user.UserRealId(); // Use your helper method instead of FindFirstValue
            if (!userId.HasValue)
                return TypedResults.Unauthorized(); // Return Unauthorized if user is not authenticated

            // Retrieve the post by ID
            var post = await repository.GetPostByIdAsync(postId);
            if (post == null)
                return TypedResults.NotFound("Post not found.");

            // Set the AuthorId for the comment from the logged-in user's Id
            comment.AuthorId = userId.ToString();

            // Add the comment to the post using the repository method, which now accepts ClaimsPrincipal user
            await repository.AddCommentToPost(postId, comment, user);

            return TypedResults.Ok("Comment added successfully.");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetCommentsForPost(int postId, [FromServices] IBlogPostRepository repository)
        {
            var comments = await repository.GetCommentsForPost(postId);
            return TypedResults.Ok(comments);
        }
    }
}
