using auth.Helpers;
using auth.Model;
using auth.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace auth.Endpoints
{
    public static class AdminEndpoints
    {

        public static void ConfigureAdminEndpoints(this WebApplication app)
        {
            var admin = app.MapGroup("/admin");
            admin.MapGet("/comments", GetComments);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize (Roles = "Admin")]
        public static async Task<IResult> GetComments([FromServices] UserManager<User> userManager, ICommentRepository commentRepository, HttpContext httpContext)
        {
            try
            {
                var comments = await commentRepository.GetAdminComments();
                return TypedResults.Ok(comments);
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}