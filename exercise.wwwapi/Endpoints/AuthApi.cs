using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO;
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
    public static class AuthApi
    {
        public static void ConfigureAuthApi(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);

            app.MapGet("posts", GetPosts);
            //app.MapPost("add" AddBlogPost);


        }

        private static async Task<IResult> GetUsers(IDatabaseRepository<User> service)
        {
            return Results.Ok(service.GetAll());
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDTO request, IDatabaseRepository<User> service)
        {

            //user exists
            if (service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequestDTO>() { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User();

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            service.Create(user);
            service.Save();

            return Results.Ok(new Payload<string>() { data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDTO request, IDatabaseRepository<User> service, IConfigurationSettings config)
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

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]

        private static async Task<IResult> GetPosts(IDatabaseRepository<BlogPost> repository)
        {
            return TypedResults.Ok(repository.GetAll());
        }


        
       private static async Task<IResult> AddBlogPost(IDatabaseRepository<BlogPost> repository, ClaimsPrincipal user, BlogPostDTO model)
        {
            var userId = user.UserRealId();

            if (userId == null)
            {
                return Results.Unauthorized();

            }

            repository.Create(new BlogPost
            {
                Text = model.Text,
                UserId = (int) userId,
            });

            



         

    }
}

    





    

