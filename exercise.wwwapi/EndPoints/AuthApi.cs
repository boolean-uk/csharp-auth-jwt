using exercise.wwwapi.Configuration;
using exercise.wwwapi.Helpers;
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
            app.MapPut("posts", UpdatePost);

            app.MapPost("user/{id}/follows/{followId}", Follow);
            app.MapPost("user/{id}/unfollows/{unfollowId}", Unfollow);
            app.MapGet("viewall/{id}", ViewFollowingPosts);

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> ViewFollowingPosts(IRepository<Following> service, IRepository<User> userservice, IRepository<BlogPost> bservice, ClaimsPrincipal user, int id)
        {
            var usertarget = userservice.GetAll().Where(u => u.Id == id).First();
                if (usertarget == null) return TypedResults.NotFound("User was not found");

            var follow = service.GetAll().Where(u => u.UserId == id.ToString());
            List<BlogPost> posts = new List<BlogPost>();
            foreach(var following in follow)
            {
                var postTarget = bservice.GetAll().Where(u => u.AuthorId == following.FollowsId);
                posts.AddRange(postTarget);
            }
            return TypedResults.Ok(new Payload<List<BlogPost>>() { data = posts });
            }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Unfollow(IRepository<Following> service, IRepository<User> userservice, ClaimsPrincipal user, int id, string unfollowId)
        {
            var target = userservice.GetAll().Where(b => b.Id == id).First();
            if (target == null) return TypedResults.NotFound("User was not found");


            var follow = service.GetAll().Where(u => u.UserId == id.ToString() && u.FollowsId == unfollowId).First();
            service.Delete(follow.Id);
            service.Save(); 
            return TypedResults.Ok(new Payload<string>() { data = $"No longer following {unfollowId}" });
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Follow(IRepository<Following> service, IRepository<User> userservice, ClaimsPrincipal user, int id, string followId)
        {
            var target =  userservice.GetAll().Where(b => b.Id == id).First();
            if (target == null) return TypedResults.NotFound("User was not found");

            Following following = new Following
            {
                UserId = id.ToString(),
                FollowsId = followId
            };
            service.Insert(following);
            service.Save();
            return TypedResults.Ok(new Payload<string>() { data = $"{id} is now following {followId}" });
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> AddPost(IRepository<BlogPost> service, ClaimsPrincipal user, BlogPostPost model)
        {
            BlogPost blogPost = new BlogPost
            {
                Text = model.Text,
                AuthorId = user.UserRealId().ToString()
            };
            service.Insert(blogPost);
            service.Save();
            return TypedResults.Ok(new Payload<string>() { data = "Post created" });
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdatePost(IRepository<BlogPost> service, ClaimsPrincipal user,int id, BlogPostPut model)
        {
            var target = service.GetAll().Where(b => b.Id == id).First();
            if (target == null) return TypedResults.NotFound("Post was not found");

            if(model.Text != null) target.Text = model.Text;
            if(model.AuthorId != null) target.AuthorId = model.AuthorId.ToString();
            service.Update(target);
            service.Save();
            return TypedResults.Ok(new Payload<string>() { data = "Post updated" });
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<BlogPost> service, ClaimsPrincipal user)
        {
            return TypedResults.Ok(service.GetAll());
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> service, ClaimsPrincipal user)
        {
            return TypedResults.Ok(service.GetAll());
        }
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
    }
}
