using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

using WebApplication1.Enums;
using WebApplication1.Models;
using WebApplication1.Models.DTO;
using WebApplication1.Repository;
using WebApplication1.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using WebApplication1.Helpers;

namespace WebApplication1.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var taskGroup = app.MapGroup("auth");
            taskGroup.MapPost("/register", Register);
            taskGroup.MapPost("/login", Login);
            taskGroup.MapGet("/blogposts/", GetAllPosts);
            taskGroup.MapGet("/blogposts/{id}/", GetAllPostsID);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async static Task<IResult> Register(RegisterDto registerPayload,
        UserManager<AuthUser> userManager)
        {
            if (registerPayload.Email == null) return
            TypedResults.BadRequest("Email is required.");

            if (registerPayload.Password == null) return
            TypedResults.BadRequest("Password is required.");

            var result = await userManager.CreateAsync(
            new AuthUser
            {
                UserName = registerPayload.Email,
                Email = registerPayload.Email,
                Role = UserRole.User
            },
            registerPayload.Password!
            );

            if (result.Succeeded)
            {
                return TypedResults.Created($"/auth/", new
                RegisterResponseDto(registerPayload.Email, UserRole.User));
            }
            return Results.BadRequest(result.Errors);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]

        public async static Task<IResult> Login(LoginDto
                loginPayload,
                UserManager<AuthUser> userManager,
                TokenService tokenService,
                Irepository repository)
        {
            if (loginPayload.Email == null) return
            TypedResults.BadRequest("Email is required.");
            if (loginPayload.Password == null) return
            TypedResults.BadRequest("Password is required.");
            // load the user from database
            var user = await
            userManager.FindByEmailAsync(loginPayload.Email!);
            if (user == null)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }

            var isPasswordValid = await
            userManager.CheckPasswordAsync(user, loginPayload.Password);
            if (!isPasswordValid)
            {
                return TypedResults.BadRequest("Invalid email or password.");
            }
            var token = tokenService.CreateToken(user);
            // return the response
            return TypedResults.Ok(new AuthResponseDto(token, user.Email, user.Role));
        }

        
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static async Task<IResult> GetAllPosts(Irepository blogs) //, ClaimsPrincipal user)
        {
            var blogPosts = await blogs.GetBlogPosts();
            List<BlogPostResponseDTO> blogPostsToReturn = new List<BlogPostResponseDTO>();

            foreach (var blogPost in blogPosts)
            {
                BlogPostResponseDTO blog = new BlogPostResponseDTO(blogPost);

                blogPostsToReturn.Add(blog);
            }

            return TypedResults.Ok(blogPostsToReturn);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize()]
        public static async Task<IResult> GetAllPostsID(Irepository blogs, ClaimsPrincipal user)
        {

            var userId = user.UserId();
            if (userId == null) return Results.Unauthorized();

            var blogPosts = await blogs.GetBlogPosts();
            List<BlogPostResponseDTO> blogPostsToReturn = new List<BlogPostResponseDTO>();

            foreach (var blogPost in blogPosts)
            {
                BlogPostResponseDTO blog = new BlogPostResponseDTO(blogPost);

                blogPostsToReturn.Add(blog);
            }

            return TypedResults.Ok(blogPostsToReturn);
        }
    }

}
