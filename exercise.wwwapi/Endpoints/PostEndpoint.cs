using exercise.wwwapi.BlogPostDTO;
using exercise.wwwapi.CommentDTO;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class PostEndpoint
    {
        public static void ConfigurePostEndpoint(this WebApplication app)
        {
            var posts = app.MapGroup("posts");
            posts.MapGet("/", GetPosts);
            posts.MapGet("/{id}", GetPostById);
            posts.MapPost("/", CreatePost);
            posts.MapPut("/{id}", UpdatePost);
            posts.MapPost("/comment", CreateComment);
            posts.MapGet("/withcomments", GetPostsWithComments);

        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<Post> repository)
        {
            var posts = repository.GetAll();
            var postsDTO = posts.Select(p => new PostResponseDto(p));
            var response = new Payload<IEnumerable<PostResponseDto>> { Data = postsDTO };
            return Results.Ok(response);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPostById(IRepository<Post> repository, int id)
        {
            var post = repository.GetById(id);

            if (post == null) return TypedResults.NotFound();

            var response = new Payload<PostResponseDto> { Data = new PostResponseDto(post) };

            return TypedResults.Ok(response);
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IRepository<Post> repository, PostRequest post, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            if (!post.Text.Any()) return TypedResults.BadRequest();

            var addPost = new Post { Text = post.Text, AuthorId = userId.Value };
            addPost = repository.Add(addPost);
            var response = new Payload<PostResponseDto> { Data = new PostResponseDto(addPost) };
            return TypedResults.Ok(response);

        }

        
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdatePost(IRepository<Post> repository, int id, PostPutRequest post, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var updatePost = repository.GetById(id);

            if (updatePost == null) { return TypedResults.NotFound("Blogpost doesn't ex"); }

            if (updatePost.AuthorId == userId)
            {
                updatePost.Text = post.Text;
                updatePost = repository.Update(updatePost);
                var response = new Payload<PostResponseDto>() { Data = new PostResponseDto(updatePost) };
                return TypedResults.Ok(response);
            }

            return Results.Unauthorized();
        }



        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateComment(IRepository<Comment> commentrepo, IRepository<Post> postrepo, CommentRequest comment, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var checkPost = postrepo.GetById(comment.PostId);

            if (checkPost == null) return TypedResults.NotFound("Blogpost does not exist");

            var addComment = new Comment() { Content = comment.Comment, PostId = comment.PostId, Userid = userId.Value };

            addComment = commentrepo.Add(addComment);

            var response = new Payload<CommentResponseDto> { Data = new CommentResponseDto(addComment) };

            return TypedResults.Ok(response);

        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPostsWithComments(IRepository<User> service, IRepository<Comment> commentrepo, IRepository<Post> postrepo, ClaimsPrincipal user)
        {
            Payload<IEnumerable<PostCommentResponseDto>> response = new();

            List<PostCommentResponseDto> allPostsWithComments = new();

            var allPosts = postrepo.GetAll();

            foreach (var post in allPosts)
            {
                if (post.Comments != null)
                {
                    PostCommentResponseDto postCommentDto = new PostCommentResponseDto() { Post = new PostResponseDto(post) };

                    foreach (var comment in post.Comments)
                    {
                        postCommentDto.Comments.Add(new CommentResponseDto(comment));
                    }

                    allPostsWithComments.Add(postCommentDto);
                }
            }

            response = new Payload<IEnumerable<PostCommentResponseDto>> { Data = allPostsWithComments };

            return TypedResults.Ok(allPostsWithComments);
        }

    }
}
