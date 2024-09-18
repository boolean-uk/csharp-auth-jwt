using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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
            app.MapPost("user/{followerId:int}/follows/{userId:int}", FollowUser);

            app.MapGet("posts", GetBlogPosts);
            app.MapPost("posts", CreateBlogPost);
            app.MapPut("posts/{id:int}", UpdateBlogPost);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IDatabaseRepository<User> service)
        {
            var users = service.GetAll();

            return Results.Ok(users);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> FollowUser(IDatabaseRepository<User> service, int userId, int followerId)
        {
            User follower = service.GetById(followerId);
            User userToFollow = service.GetById(userId);

            if(follower == null || userToFollow == null)
            {
                return Results.BadRequest(new Payload<string>{ status = "User does not exist", data = "" });
            }

            if (follower.Follows.Contains(userToFollow))
            {
                return Results.BadRequest(new Payload<User> { status = "User is already following", data = userToFollow });
            }

            follower.Follows.Add(userToFollow);
            service.Update(follower);
            service.Save();

            return Results.Ok(new Payload<string> { status = "User followed ", data = userToFollow.Username });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> UnFollowuser(IDatabaseRepository<User> service, int userId, int followerId)
        {
            User follower = service.GetById(followerId);
            User userToUnFollow = service.GetById(userId);

            if (follower == null || userToUnFollow == null)
            {
                return Results.BadRequest(new Payload<string> { status = "User does not exist", data = "" });
            }
            if (!follower.Follows.Contains(userToUnFollow)) 
            {
                return Results.BadRequest(new Payload<User> { status = "You are not following this user", data = userToUnFollow });
            }

            follower.Follows.Remove(userToUnFollow);
            service.Update(follower);
            service.Save();

            return Results.Ok(new Payload<string> { status = "User followed ", data = userToUnFollow.Username });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> GetAllFollowers(IDatabaseRepository<User> service, int id)
        {
            User user = service.GetAll(u => u.Follows).FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                Results.BadRequest("User does not exist");
            }

            return Results.Ok(new Payload<User> { status = "Success", data = user });
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
            return Results.Ok(new Payload<string>() { data =  token }) ;
           
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogPosts(IDatabaseRepository<BlogPost> service)
        {
            return Results.Ok(service.GetAll());
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateBlogPost(IDatabaseRepository<BlogPost> service, PostBlogPostDto postModel)
        {
            var post = new BlogPost();
            post.Title = postModel.Title;
            post.Content = postModel.Content;
            post.AuthorId = postModel.AuthorId;

            service.Insert(post);
            service.Save();
            return Results.Ok( new Payload<PostBlogPostDto>() { data = postModel , status="success" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        private static async Task<IResult> UpdateBlogPost(IDatabaseRepository<BlogPost> service, int id, PostBlogPostDto postModel)
        {
            BlogPost post = service.GetById(id);

            post.Title = postModel.Title;
            post.Content = postModel.Content;
            post.AuthorId = postModel.AuthorId;

            service.Update(post);
            service.Save();
            return Results.Ok(new Payload<PostBlogPostDto>() { data = postModel, status = "success" });
        }

        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
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
    }
}

