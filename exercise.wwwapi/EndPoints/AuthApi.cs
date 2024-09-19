using exercise.wwwapi.Configuration;
using exercise.wwwapi.Data;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.EndPoints
{
    public static class AuthApi
    {
        public static void ConfigureAuthApi(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);

            app.MapGet("posts", GetPosts);
            app.MapPost("posts", AddPost);
            app.MapPut("posts/{id}", UpdatePost);

            app.MapPost("users/follows/{followId}", Follow);
            app.MapPost("users/{userId}/unfollows/{followId}", Unfollow);
            app.MapGet("followerposts/", ViewAll);

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IDatabaseRepository<User> service)
        {
            return Results.Ok(service.GetAll());
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDto request, IDatabaseRepository<User> service)
        {

            //user exists
            if (service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequestDto>() { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User();

            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            service.Insert(user);
            service.Save();

            return Results.Ok(new Payload<string>() { data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDto request, IDatabaseRepository<User> service, IConfigurationSettings config)
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
                new Claim(ClaimTypes.Name, user.Username)
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


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IDatabaseRepository<BlogPost> service)
        {
            return Results.Ok(service.GetAll());
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> AddPost(BlogPostRequestDto request, ClaimsPrincipal user, IDatabaseRepository<BlogPost> service)
        {

            var post = new BlogPost();
            var userId = user.UserRealId(); 


            post.Text = request.Text;
            post.AuthorId = $"{userId}";


            service.Insert(post);
            service.Save();

            return Results.Ok(new Payload<string>() { data = "Post created" });
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> UpdatePost(BlogPostRequestDto request, IDatabaseRepository<BlogPost> service, int id)
        {
            var post = service.GetById(id);

            post.Text = string.IsNullOrEmpty(request.Text) ? post.Text : request.Text;
            //post.AuthorId = string.IsNullOrEmpty(request.AuthorId) ? post.AuthorId : request.AuthorId;

            service.Update(post);
            service.Save();

            return Results.Ok(new Payload<string>() { data = "Post updated" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Follow(IDatabaseRepository<Follow> service, ClaimsPrincipal user, int id)
        {
            Follow follow = new Follow();

            var userId = user.UserRealId();

            follow.FollowerId = (int)userId;
            follow.UserId = id;

            service.Insert(follow);
            service.Save();

            return Results.Ok(new Payload<string>() { data = $"User {userId} now follows {id}" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Unfollow(IDatabaseRepository<Follow> service, ClaimsPrincipal user, int id)
        {
            var userId = user.UserRealId();

            Follow follow = service.GetAll().FirstOrDefault(x => x.FollowerId == userId && x.UserId == id);

            service.Delete(follow.Id);
            service.Save();


            return Results.Ok(new Payload<string>() { data = $"User {userId} unfollowed user {id}" });
        }

        

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> ViewAll(IDatabaseRepository<User> userService, IDatabaseRepository<Follow> followService, ClaimsPrincipal user)
        {
            var userId = user.UserRealId();

            List<Follow> followers = followService.GetAll().Where(x => x.FollowerId == userId).ToList();

            List<BlogPost> followersPosts = new List<BlogPost>();

            foreach (var follower in followers)
            {
                var followerInfo = userService.GetById(userId);
                var posts = followerInfo.Posts;

                followersPosts.AddRange(posts);
            }

            return Results.Ok(followersPosts);
        }
        


    }
}


