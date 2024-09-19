using exercise.wwwapi.DTOs;
using exercise.wwwapi.DTOs.BlogpostDTOs;
using exercise.wwwapi.DTOs.CommentDTOs;
using exercise.wwwapi.Helper;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogPostEndpoint
    {
        public static void ConfigureBlogPostEndpoint(this WebApplication app)
        {
            var blogposts = app.MapGroup("posts");
            blogposts.MapGet("/", GetBlogPosts);
            blogposts.MapPost("/", AddBlogPost);
            blogposts.MapPut("/", EditBlogPost);

            blogposts.MapPost("/comment", AddComment);
            blogposts.MapGet("/withcomments", GetPostsWithComments);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogPosts(IDatabaseRepository<User> service, IDatabaseRepository<BlogPost> blogrepo, ClaimsPrincipal user)
        {
            var blogposts = blogrepo.GetAll();
            List<BlogpostResponseDTO> allposts = new List<BlogpostResponseDTO>();

            foreach (var post in blogposts)
            {
                var username = service.GetById(post.authorId);
                BlogpostResponseDTO response = new BlogpostResponseDTO();
                response.Blogpostid = post.Id;
                response.Text = post.Text;
                response.Author = username.Username;
                allposts.Add(response);

            }

            return Results.Ok(allposts);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> AddBlogPost(BlogpostRequestDTO request, IDatabaseRepository<BlogPost> blogrepo, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var post = new BlogPost();
            post.Text = request.Text;
            post.authorId = userId.Value;

            blogrepo.Insert(post);
            blogrepo.Save();

            BlogpostResponseDTO response = new BlogpostResponseDTO() { Blogpostid = post.Id, Text = post.Text, Author = user.UserName() };

            return Results.Ok(new Payload<BlogpostResponseDTO>() { data = response });


        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> EditBlogPost(BlogpostPutRequest request, IDatabaseRepository<BlogPost> blogrepo, IDatabaseRepository<User> service, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var checkBlogpost = blogrepo.GetById(request.BlogpostId);
            if (checkBlogpost == null)
            {
                return Results.NotFound("Blogpost does not exist");
            }

            if (checkBlogpost.authorId == userId)
            {
                if (request.Text == "string") request.Text = string.Empty;
                if (request.AuthorId == 0) request.AuthorId = null;
                checkBlogpost.Text = !string.IsNullOrEmpty(request.Text) ? request.Text : checkBlogpost.Text;
                checkBlogpost.authorId = request.AuthorId != null ? request.AuthorId.Value : checkBlogpost.authorId;

                blogrepo.Update(checkBlogpost);
                blogrepo.Save();

                var username = service.GetById(checkBlogpost.authorId);
                BlogpostResponseDTO response = new BlogpostResponseDTO();
                response.Blogpostid = checkBlogpost.Id;
                response.Text = checkBlogpost.Text;
                response.Author = username.Username;
                return Results.Ok(response);
            }
            return Results.Unauthorized();

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> AddComment(CommentPostDTO request, IDatabaseRepository<Comment> commentrepo, IDatabaseRepository<BlogPost> blogrepo, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var checkBlogPost = blogrepo.GetById(request.BlogPostId);
            if (checkBlogPost == null)
            {
                return TypedResults.NotFound("Blogpost does not exist");
            }

            var comment = new Comment() { CommentText = request.Comment, BlogPostId = checkBlogPost.Id, Userid = userId.Value }; //, BlogPost = checkBlogPost

            commentrepo.Insert(comment);
            commentrepo.Save();
            checkBlogPost.Comments.Add(comment);

            CommentDTO commentDTO = new CommentDTO();
            commentDTO.Comment = comment.CommentText;
            commentDTO.WrittenBy = user.UserName();

            return TypedResults.Ok(commentDTO);

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPostsWithComments(IDatabaseRepository<User> service, IDatabaseRepository<Comment> commentrepo, IDatabaseRepository<BlogPost> blogrepo, ClaimsPrincipal user)
        {
            var blogposts = blogrepo.GetAll();
            List<PostsWithCommentsDTO> postswithcomments = new List<PostsWithCommentsDTO>();

            foreach (var post in blogposts)
            {
                var username = service.GetById(post.authorId);
                BlogpostResponseDTO blogpostDTO = new BlogpostResponseDTO();
                PostsWithCommentsDTO postresponseDTO = new PostsWithCommentsDTO();
                blogpostDTO.Blogpostid = post.Id;
                blogpostDTO.Text = post.Text;
                blogpostDTO.Author = username.Username;

                postresponseDTO.BlogPost = blogpostDTO;

                if (post.Comments != null)
                {
                    var allcomments = commentrepo.GetAll();
                    var postcomments = allcomments.Where(x => x.BlogPostId == post.Id);

                    foreach (var comment in postcomments)
                    {
                        var commentuser = service.GetById(comment.Userid);
                        CommentDTO commentDTO = new CommentDTO();
                        commentDTO.Comment = comment.CommentText;
                        commentDTO.WrittenBy = commentuser.Username;

                        postresponseDTO.Comments.Add(commentDTO);
                    }

                }

                postswithcomments.Add(postresponseDTO);

            }

            return Results.Ok(postswithcomments);
        }
    }
}
