using exercise.wwwapi.DTO;
using exercise.wwwapi.Helper;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class SecureApi
    {
        public static void ConfigureUserEndpoint(this WebApplication app)
        {
            var userGroup = app.MapGroup("users");
            userGroup.MapGet("/", GetUsers);

            var postGroup = app.MapGroup("posts");
            postGroup.MapGet("/", GetPosts);
            postGroup.MapPost("/", CreatePost);
            postGroup.MapPut("/{id}", UpdatePost);

            app.MapPost("/register", AuthApi.Register);
            app.MapPost("/login", AuthApi.Login);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> repository, ClaimsPrincipal user)
        {
            var users = await repository.GetAll();
            return TypedResults.Ok(users);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<BlogPost> repository, ClaimsPrincipal user)
        {
            var posts = await repository.GetAll();
            return TypedResults.Ok(posts);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IRepository<BlogPost> postRepo, IRepository<User> userRepo, BlogPostDTO postDTO, ClaimsPrincipal user)
        {
            int? userId = user.UserRealId();

            var users = await userRepo.GetAll();
            User? author = users.FirstOrDefault(u => u.Id == userId);

            if (author == null)
                return Results.BadRequest(new Payload<string> { status = "User not found" });

            BlogPost post = new BlogPost
            {
                Text = postDTO.Text,
                AuthorId = userId.Value,
                Author = author
            };

            postRepo.Insert(post);

            return TypedResults.Ok(new Payload<string> { data = "Post Created" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdatePost(
            IRepository<BlogPost> postRepo,
            IRepository<User> userRepo,
            BlogPostDTO postDTO,
            ClaimsPrincipal user,
            int id)
            {
            int? userId = user.UserRealId();
            if (userId == null)
                return Results.Unauthorized();

            // Hent posten fra databasen (bruk AsNoTracking for å unngå problemer med ChangeTracker)
            BlogPost? existingPost = (await postRepo.GetAll()).FirstOrDefault(p => p.Id == id);

            if (existingPost == null)
                return Results.NotFound(new Payload<string> { status = "Post not found" });

            if (existingPost.AuthorId != userId)
                return Results.Forbid(); // HTTP 403 hvis brukeren ikke er eieren av innlegget

            existingPost.Text = postDTO.Text;

            // Bruk den oppdaterte Update-metoden som er async
            await postRepo.Update(existingPost);

            return TypedResults.Ok(new Payload<string> { data = "Post Updated Successfully" });
        }
    }

    }

