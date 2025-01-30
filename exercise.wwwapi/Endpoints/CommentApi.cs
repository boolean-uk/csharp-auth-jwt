using System.Security.Claims;
using AutoMapper;
using exercise.wwwapi.DTO.Request;
using exercise.wwwapi.DTO.Response;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class CommentApi
    {
        public static void ConfigureCommentApi(this WebApplication app)
        {
            var comments = app.MapGroup("/posts/{postId}/comments");
            app.MapGet("/postswithcomments", GetPostsWithComments);
            comments.MapGet("/", GetComments);
            comments.MapPost("/", CreateComment);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPostsWithComments(IRepository<BlogPost> postService, IRepository<Comment> commentService, IMapper mapper)
        {
            var posts = await postService.GetWithIncludes(p => p.User, p => p.Comments);
            var postDtos = mapper.Map<IEnumerable<BlogPostWithCommentsDto>>(posts);
            var response = new Payload<IEnumerable<BlogPostWithCommentsDto>>() { data = postDtos };
            return TypedResults.Ok(response);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetComments(IRepository<Comment> service, int postId, IMapper mapper)
        {
            var comments = await service.GetWithIncludes(c => c.User, c => c.BlogPost);
            var postComments = comments.Where(c => c.BlogPostId == postId);
            var dtos = mapper.Map<IEnumerable<CommentResponseDto>>(postComments);
            var response = new Payload<IEnumerable<CommentResponseDto>>() { data = dtos };
            return TypedResults.Ok(response);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateComment(IRepository<Comment> service, IMapper mapper, CommentRequestDto commentRequest, int postId, ClaimsPrincipal claim)
        {
            var comment = mapper.Map<Comment>(commentRequest);
            if (claim.UserRealId() != null)
            {
                comment.UserId = (int)claim.UserRealId();
                comment.BlogPostId = postId;
                service.Insert(comment);
                service.Save();
                var commentWithUser = await service.GetByIdWithIncludes(comment.Id, c => c.User);
                var commentResponse = mapper.Map<CommentResponseDto>(commentWithUser);
                commentResponse.Username = commentWithUser.User.Username;
                return TypedResults.Ok(new Payload<CommentResponseDto>() { data = commentResponse });
            }
            return TypedResults.Unauthorized();
        }
    }
}
