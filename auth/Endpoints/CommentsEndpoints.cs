using System;
using System.Net.Http;
using System.Threading.Tasks;
using auth.Helpers;
using auth.Model;
using auth.Model.Payload;
using auth.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace auth.Endpoints
{
    public static class CommentsEndpoints
    {

        public static void ConfigureCommentsEndpoints(this WebApplication app)
        {
            var comments = app.MapGroup("/comments");
            comments.MapGet("/", GetComments);
            comments.MapPost("/", AddComment);
            comments.MapDelete("/{id}", DeleteComment);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize]
        public static async Task<IResult> GetComments([FromServices] UserManager<User> userManager, ICommentRepository commentRepository, HttpContext httpContext)
        {
            try
            {
                string UserID = ClaimsPrincipalHelpers.GetUserId(httpContext.User);
                var comments = await commentRepository.GetComments(UserID);
                return TypedResults.Ok(comments);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public static async Task<IResult> AddComment([FromServices] UserManager<User> userManager, ICommentRepository commentRepository, CreateCommentPayload payload, HttpContext httpContext)
        {
            try
            {
                string UserID = ClaimsPrincipalHelpers.GetUserId(httpContext.User);
                Console.WriteLine(UserID);
                Comment comment = await commentRepository.AddComment(payload.Text, UserID);
                return TypedResults.Created("Created", comment);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        //Admin or user who created it can delete...

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize]
        public static async Task<IResult> DeleteComment([FromServices] UserManager<User> userManager, ICommentRepository commentRepository, int id, HttpContext httpContext)
        {
            try
            {
                var comment = await commentRepository.GetCommentById(id);

                if (comment == null)
                {
                    return Results.NotFound("Comment not found.");
                }
                string UserID = ClaimsPrincipalHelpers.GetUserId(httpContext.User);
                bool isAdmin = httpContext.User.IsInRole("Admin");
                Console.WriteLine(UserID);
                Console.WriteLine(comment.UserId);
                Console.WriteLine(isAdmin);
                if (UserID == comment.UserId || isAdmin)
                {
                    await commentRepository.DeleteComment(id);
                    return Results.Ok();
                }
                else
                {
                    return Results.Forbid();
                }
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }

}
