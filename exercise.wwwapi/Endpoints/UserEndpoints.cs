using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoints
    {
        public static void ConfigureUserEndpoint(this WebApplication app)
        {
            var userGroup = app.MapGroup("users");
            userGroup.MapGet("/", GetUsers);
            userGroup.MapPut("/{userId}/follow/{targetUserId}", FollowUser);

            var postGroup = app.MapGroup("posts");
            postGroup.MapGet("/", GetPosts);
            postGroup.MapPost("/", CreatePost);
            postGroup.MapPut("/{id}", UpdatePost);

            app.MapPost("/register", Register);
            app.MapPost("/login", Login);
        }

        // Security

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDto request, IRepository<User> service)
        {
            //user exists
            if (service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequestDto>() { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User();

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.Email = request.Email;

            service.Insert(user);
            service.Save();

            return Results.Ok(new Payload<string>() { data = "Created Account" });
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDto request, IRepository<User> service, IConfigurationSettings config)
        {
            //user doesn't exist
            if (!service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.BadRequest(new Payload<UserRequestDto>() { status = "User does not exist", data = request });

            User user = service.GetAll().FirstOrDefault(u => u.Username == request.Username)!;


            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserRequestDto>() { status = "Wrong Password", data = request });
            }
            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string>() { data = token });

        }

        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue("AppSettings:Token")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        // Endpoints:
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> service, ClaimsPrincipal user)
        {
            var users = service.GetAll(u => u.Followers).ToList();


            return TypedResults.Ok(users);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<Post> service, ClaimsPrincipal user)
        {
            return TypedResults.Ok(service.GetAll());
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreatePost(IRepository<Post> postService, IRepository<User> userService, PostDTO postDTO, ClaimsPrincipal user)
        {
            int? userId = user.UserRealId();
            User? author = userService.GetAll().FirstOrDefault(u => u.Id == userId.Value);
            if (author == null)
            {
                return Results.BadRequest(new Payload<string> { status = "User not found" });
            }
            Post post = new Post()
            {
                Text = postDTO.Text,
                AuthorId = user.UserRealId().Value,
                Author = author
            };
            postService.Insert(post);
            postService.Save();

            return TypedResults.Ok(new Payload<string> { data = "Post Created" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdatePost(IRepository<Post> postService, IRepository<User> userService, PostDTO postDTO, ClaimsPrincipal user, int postId)
        {
            int? userId = user.UserRealId();

            if (userId == null)
            {
                return Results.Unauthorized();
            }

            Post? existingPost = postService.GetAll().FirstOrDefault(p => p.Id == postId);

            if (existingPost == null)
            {
                return Results.NotFound(new Payload<string> { status = "Post not found" });
            }

            if (existingPost.AuthorId != userId.Value)
            {
                return Results.Forbid();
            }

            existingPost.Text = postDTO.Text;
            postService.Update(existingPost);
            postService.Save();

            return TypedResults.Ok(new Payload<string> { data = "Post Updated Successfully" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> FollowUser(IRepository<Post> postService, IRepository<User> userService, ClaimsPrincipal user, int userId, int targetUserId)
        {
            int? userRealId = user.UserRealId();
            User? userToAdd = userService.GetAll().FirstOrDefault(u => u.Id == userRealId.Value);

            if (userId == null)
            {
                return Results.Unauthorized();
            }

            if(userId == targetUserId)
            {
                return Results.BadRequest("You cannot follow yourself");
            }

            var targetUser = userService.GetAll().FirstOrDefault(u => u.Id == targetUserId);

            if (targetUser == null)
            {
                return Results.NotFound("Target user not found");
            }

            targetUser.Followers.Add(userToAdd);
            userService.Update(targetUser);
            userService.Save();

            return TypedResults.Ok(new Payload<string> { data = $"{userToAdd.Username} is now following {targetUser.Username}" });
        }

    }
}
