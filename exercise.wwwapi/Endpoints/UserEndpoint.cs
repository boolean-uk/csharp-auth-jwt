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
using System.Xml;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoint
    {
        public static void ConfigureUserEndpoints(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);
            var user = app.MapGroup("user");
            user.MapGet("/blogpost{id}", GetBlogpost);
            user.MapGet("/blogposts", GetBlogposts);
            user.MapPost("/blogpost", CreateBlogpost);
            user.MapPut("/blogpost", UpdateBlogpost);
            user.MapPut("/update", UpdateUser);
        }



        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdateUser(IRepository<User> service)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UpdateBlogpost(IRepository<User> service)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> CreateBlogpost(IRepository<BlogPost> service, HttpContext context, BlogPost blogpost)
        {
            var newBlogpost = new BlogPost();
            if (service.GetAll().Count() == 0)
            {
                newBlogpost.Id = 1;
            }
            else
            {
                newBlogpost.Id = service.GetAll().Max(x => x.Id) + 1;
            }
            newBlogpost.Content = blogpost.Content;
            //newBlogpost.authorId = 
            var name = context.User.Identity.Name;

            service.Insert(blogpost);
            service.Save();
            return Results.Ok(new Payload<string>() { data = $"Created Blogpost {name}" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogpost(IRepository<User> service)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetBlogposts(IRepository<User> service)
        {
            throw new NotImplementedException();
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> service)
        {
            return Results.Ok(service.GetAll());
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDTO request, IRepository<User> service)
        {

            //user exists
            if (service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequestDTO>() { status = "Username already exists!", data = request });

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
        private static async Task<IResult> Login(UserRequestDTO request, IRepository<User> service, IConfigurationSettings config)
        {
            //user doesn't exist
            if (!service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.BadRequest(new Payload<UserRequestDTO>() { status = "User does not exist", data = request });

            User user = service.GetAll().FirstOrDefault(u => u.Username == request.Username)!;


            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserRequestDTO>() { status = "Wrong Password", data = request });
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
    }
}
