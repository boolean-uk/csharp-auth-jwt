using exercise.wwwapi.Models;
using exercise.wwwapi.Models.DataTransferObjects;
using exercise.wwwapi.Models.DataTransferObjects.RequestDTO;
using exercise.wwwapi.Models.DataTransferObjects.ResponseDTO;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogApi
    {
        public static async void ConfigureBlogApi(this WebApplication app)
        {
            var blog = app.MapGroup("/blog");

            // Get posts
            blog.MapGet("posts", GetPosts);
            blog.MapGet("posts/{id}", GetPostsById);
            blog.MapGet("posts/following/{id}", GetPostsByFollowing);

            // Create - Delete - Edit
            blog.MapPost("posts/create/", CreatePost);
            blog.MapDelete("posts/delete/{id}", DeletePost);
            blog.MapPut("posts/edit/{id}", EditPost);

            // Comment
            blog.MapPost("posts/comment", CreateComment);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetPosts(IRepository<Blogpost> repo)
        {
            IEnumerable<Blogpost> posts = repo.GetAll();
            Payload<IEnumerable<BlogpostResponseDTO>> payload = new Payload<IEnumerable<BlogpostResponseDTO>>()
            {
                status = "success",
                data = posts.Select(p => new BlogpostResponseDTO(p))

            };
            return Results.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetPostsById(IRepository<Blogpost> repo, int id)
        {
            IEnumerable<Blogpost> posts = repo.GetAll();
            var responseData = posts.Where(p => p.AuthorId == id).ToList();

            Payload<IEnumerable<BlogpostResponseDTO>> payload = new Payload<IEnumerable<BlogpostResponseDTO>>()
            {
                status = "success",
                data = responseData.Select(p => new BlogpostResponseDTO(p))
               
            };
            return Results.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetPostsByFollowing(IRepository<Blogpost> blogRepo, IRepository<Relation> relRepo, int id)
        {
            IEnumerable<Blogpost> posts = blogRepo.GetAll();
            IEnumerable<int> relations = relRepo.GetAll().Where(r => r.FollowerId == id).Select(r => r.FollowedId).ToList();

            Payload<IEnumerable<BlogpostResponseDTO>> payload = new Payload<IEnumerable<BlogpostResponseDTO>>()
            {
                status = "success",
                data = posts.Where(p => relations.Contains(p.AuthorId)).ToList().Select(p => new BlogpostResponseDTO(p))
            };
            return Results.Ok(payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreatePost(IRepository<Blogpost> repo, BlogpostRequestDTO request)
        {
            Blogpost post = new Blogpost()
            {
                AuthorId = request.AuthorId,
                Title = request.Title,
                Content = request.Content,
            };
            repo.Insert(post);
            return Results.Created("/", new Payload<BlogpostRequestDTO>() { status = "success", data = request }); 
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> DeletePost(IRepository<Blogpost> repo, int id, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.Sid);
            IEnumerable<Blogpost> posts = repo.GetAll();

            if (!posts.Any(p => p.Id == id))
            {
                return Results.NotFound(new Payload<int>() { status = "No blogpost found with id", data = id });
            }

            Blogpost post = repo.GetById(id);

            if (int.Parse(userIdClaim.Value) != post.Author.Id || userIdClaim == null)
            {
                return Results.Unauthorized();
            }

            repo.Delete(id);
            return Results.Ok(new Payload<BlogpostResponseDTO>() { status = "success", data = new BlogpostResponseDTO(post) });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> EditPost(IRepository<Blogpost> repo, BlogpostRequestDTO request, int id, ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(ClaimTypes.Sid);

            IEnumerable<Blogpost> posts = repo.GetAll();
            if (!posts.Any(p => p.Id == id))
            {
                return Results.NotFound(new Payload<int>() { status = "No blogpost found with id", data = id });
            }
            
            Blogpost post = posts.FirstOrDefault(p => p.Id == id);
            
            if (int.Parse(userIdClaim.Value) != post.Author.Id || userIdClaim == null)
            {
                return Results.Unauthorized();
            }

            post.Title = request.Title;
            post.Content = request.Content;
            post.UpdatedAt = DateTime.UtcNow;

            repo.Update(post);
            return Results.Ok(new Payload<BlogpostResponseDTO>() { status = "success", data = new BlogpostResponseDTO(post)});  
            
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreateComment(IRepository<Blogpost> blogRepo, IRepository<Comment> commentRepo, CommentRequestDTO request)
        {
            IEnumerable<Blogpost> blogposts = blogRepo.GetAll();
            if (!blogposts.Any(p => p.Id == request.BlogpostId))
            {
                return Results.NotFound(new Payload<CommentRequestDTO>() { status = "No blogpost foudn with id", data = request });
            }

            Comment comment = new Comment()
            {
                Content = request.Content,
                BlogpostId = request.BlogpostId,
                UserId = request.UserId,
            };

            commentRepo.Insert(comment);
            return Results.Ok(new Payload<CommentResponseDTO>() { status = "success" , data = new CommentResponseDTO(comment) });
        }
    }
}