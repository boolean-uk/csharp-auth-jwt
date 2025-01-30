using System.Security.Claims;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Exceptions;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace exercise.wwwapi.Endpoints
{
    public static class CommentEndpoints
    {
        public static string Path { get; } = $"/{BlogPostEndpoints.Path}/{{blogPostId}}/comments";
        public static void ConfigureCommentEndpoints(this WebApplication app)
        {
            var group = app.MapGroup(Path);

            group.MapPost("/", CreateComment);
            group.MapDelete("/{commentId}", DeleteComment);
            group.MapGet("/", GetComments);
            app.MapGet("/postswithcomments", GetPostsWithComments);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> GetPostsWithComments(IRepository<BlogPost, int> repository)
        {
            try
            {
                IEnumerable<BlogPost> posts = await repository.FindAll(
                    condition: x => x.Comments.Count() > 0,
                    includeChains: q => q.Include(x => x.Comments).ThenInclude(x => x.User));
                return TypedResults.Ok(new Payload { Data = posts.Select(post => new BlogPostViewComments(
                    post.Id,
                    post.Text,
                    post.Comments.Select(comment => new CommentView(
                        comment.Id,
                        comment.Text,
                        new UserSimple(
                            comment.User.UserName!
                        )
                    )
                ))) });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> GetComments(IRepository<BlogPost, int> repository, int blogPostId)
        {
            try
            {
                BlogPost post = await repository.Get(blogPostId, q => q.Include(x => x.Comments).ThenInclude(x => x.User));
                return TypedResults.Ok(new Payload
                {
                    Data = new BlogPostViewComments(
                    post.Id,
                    post.Text,
                    post.Comments.Select(comment => new CommentView(
                        comment.Id,
                        comment.Text,
                        new UserSimple(
                            comment.User.UserName!
                        )
                    )
                ))
                });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> DeleteComment(IRepository<BlogPost, int> repository, IRepository<Comment, int> commentRepository, int blogPostId, int commentId)
        {
            try
            {
                BlogPost post = await repository.Get(blogPostId, q => q.Include(x => x.Comments).ThenInclude(x => x.User));
                Comment? comment = post.Comments.FirstOrDefault(x => x.Id == commentId);
                if (comment == null) 
                    return TypedResults.NotFound(new Payload { Status = "failure", Data = "Could not find a comment with that Id!" });

                await commentRepository.Delete(comment);

                return TypedResults.Ok(new Payload
                {
                    Data = new CommentView(
                        comment.Id,
                        comment.Text,
                        new UserSimple(comment.User.UserName!))
                });
            }
            catch (EntityNotFoundException ex)
            {
                return TypedResults.NotFound(new Payload { Status = "failure", Data = ex.Message });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        private static async Task<IResult> CreateComment(
            IRepository<BlogPost, int> repository,
            IRepository<Comment, int> commentRepository,
            IRepository<User, int> userRepository,
            ClaimsPrincipal user,
            CommentPost entity,
            int blogPostId)
        {
            try
            {
                BlogPost blogPost = await repository.Get(blogPostId);
                User authUser = await userRepository.Get(user.UserRealId()!.Value);

                Comment comment = await commentRepository.Add(new Comment
                {
                    Text = entity.Text,
                    BlogPostId = blogPost.Id,
                    UserId = authUser.Id,
                });

                blogPost = await repository.Get(blogPost.Id, q => q.Include(x => x.Comments).ThenInclude(x => x.User));

                return TypedResults.Created(Path.Replace("{blogPostId}", blogPostId.ToString()), new Payload
                {
                    Data = new BlogPostViewComments(
                        blogPost.Id,
                        blogPost.Text,
                        blogPost.Comments.Select(comment => new CommentView(
                            comment.Id,
                            comment.Text,
                            new UserSimple(
                                comment.User.UserName!
                            )
                        )))
                });
            }
            catch (EntityNotFoundException ex)
            {
                return TypedResults.NotFound(new Payload { Status = "failure", Data = ex.Message });
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }
    }
}
