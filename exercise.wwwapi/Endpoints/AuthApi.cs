using System.Security.Claims;
using exercise.wwwapi.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO;
using System.Runtime.CompilerServices;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthApi
    {

        public static void ConfigureAuthApi(this WebApplication app)
        {

            app.MapGet("/posts", GetPosts);
            app.MapPost("/posts", CreatePost);
            app.MapPut("/posts", EditPost);
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> service, ClaimsPrincipal user)
        {
            return TypedResults.Ok(service.GetAll());
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetPosts(IRepository<Post> repository, ClaimsPrincipal user)
        {
            return TypedResults.Ok(repository.GetAll());
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreatePost(IRepository<Post> repository, IRepository<User> userRepo, PostRequestDTO postDTO)
        {

            Post post = new Post()
            {
                Text = postDTO.Text,
                AuthorId = postDTO.AuthorId.ToString()
            };


            repository.Insert(post);
            repository.Save();



            return TypedResults.Ok(new Payload<string>() { data = $"{post.Text}" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> EditPost(IRepository<Post> repository, IRepository<User> userRepo, PostRequestDTO postDTO, int PostId)
        {
            Post post = repository.GetAll().Where(x => x.Id == PostId).FirstOrDefault();

            User? user = userRepo.GetById(int.Parse(post.AuthorId));

            if (user == null)
            {
                return TypedResults.NotFound();
            }

            post.AuthorId = postDTO.AuthorId.ToString();
            post.Text = postDTO.Text;

            repository.Update(post);
            repository.Save();

            return TypedResults.Ok(new Payload<string>() { data = $"{postDTO.Text}" });

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDTO request, IRepository<User> service)
        {


            if (service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequestDTO>() { status = "Username already exists!", data = request });

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
