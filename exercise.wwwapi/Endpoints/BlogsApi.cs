using exercise.wwwapi.Configuration;
using exercise.wwwapi.Model;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogsApi
    {
        public static void ConfigureBlogsApi(this WebApplication app) 
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapPost("post",CreatePost);
            app.MapGet("posts", GetAllPosts);
            app.MapPost("user/{followerId}/follows/{followingId}", FollowUser);
            app.MapPost("user/{followerId}/unfollows/{followingId}", UnfollowUser);
            app.MapGet("/viewall/{userId}", ViewFollowedPosts);
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IDatabaseRepository<Author> service)
        {
            return Results.Ok(service.GetAll());
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(AuthorReqDTO request, IDatabaseRepository<Author> service)
        {

            //user exists
            if (service.GetAll().Where(u => u.UserName == request.Username).Any()) return Results.Conflict(new Payload<AuthorReqDTO>() { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new Author();

            user.Id = Guid.NewGuid().ToString();

            user.UserName = request.Username;
            user.PasswordHash = passwordHash;

            service.Insert(user);
            service.Save();

            return Results.Ok(new Payload<Author>() { data=  user });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(AuthorReqDTO request, IDatabaseRepository<Author> service, IConfigurationSettings config)
        {
            //user doesn't exist
            if (!service.GetAll().Where(u => u.UserName == request.Username).Any()) return Results.BadRequest(new Payload<AuthorReqDTO>() { status = "User does not exist", data = request });

            Author user = service.GetAll().FirstOrDefault(u => u.UserName  == request.Username)!;


            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<AuthorReqDTO>() { status = "Wrong Password", data = request });
            }
            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string>() { data = token });

        }
        private static string CreateToken(Author user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id)
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
        private static async Task<IResult> CreatePost([FromBody] BlogPostReqDTO request, HttpContext context, IDatabaseRepository<BlogPost> postService, IDatabaseRepository<Author> authorService)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Results.Unauthorized();
            }
            var author = authorService.GetAll().FirstOrDefault(a => a.Id == userId);
            if (author == null) {
                return Results.NotFound("User Not found");
            }
            var post = new BlogPost
            {
                Text = request.Text,
                authorId = userId,
                Author = author

            };
            postService.Insert(post);
            postService.Save();

            var res = new BlogPostResDTO
            {
                Id = post.Id,
                Text = post.Text,
                AuthorId = author.Id,
                AuthorName = author.UserName
            };

            return Results.Ok(res);
        }
        [Authorize]
        private static async Task<IResult> GetAllPosts(HttpContext context, IDatabaseRepository<BlogPost> postService)
        {
            return Results.Ok(postService.GetAll().ToList());
        }
        [Authorize]
        private static async Task<IResult> FollowUser(string followerId, string followingId, IDatabaseRepository<Follow> followService, IDatabaseRepository<Author> authorService)
        {
            if (followerId == null || followerId == followerId) {
                Results.BadRequest("Not valid id");
            }
            var follower = authorService.GetById(followerId);
            var following = authorService.GetById(followingId);
            if (follower == null || following == null)
                return Results.NotFound(" not found.");

            var follow = new Follow
            {
                FollowerId = followerId,
                Follower = follower,
                FollowingId = followingId,
                Following = following
            };

            followService.Insert(follow);
            followService.Save();

            return Results.Ok("You are now following");

        }

        [Authorize]
        private static async Task<IResult> ViewFollowedPosts(string userId, IDatabaseRepository<Follow> followService, IDatabaseRepository<BlogPost> postService)
        {
            var followedUserIds = followService.GetAll()
                .Where(f => f.FollowerId == userId)
                .Select(f => f.FollowingId)
                .ToList();

            var posts = postService.GetAll()
                .Where(p => followedUserIds.Contains(p.authorId))
                .Select(post => new BlogPostResDTO
                {
                    Id = post.Id,
                    Text = post.Text,
                    AuthorId = post.authorId,
                    AuthorName = post.Author.UserName
                })
                .ToList();
            

            return Results.Ok(posts);
        }
        [Authorize]
        private static async Task<IResult> UnfollowUser(string followerId, string followingId, IDatabaseRepository<Follow> followService)
        {
            var follow = followService.GetAll().FirstOrDefault(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow == null)
                return Results.BadRequest();

            followService.Delete(follow);
            followService.Save();

            return Results.Ok("User is unfollowed");
        }

    }
}
