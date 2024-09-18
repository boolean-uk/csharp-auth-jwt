using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Endoints
{
    public static class AuthApi
    {
        public static void ConfigureAuthApi(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("authors", GetAuthors);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetAuthors(IDatabaseRepository<Author> service, ClaimsPrincipal author)
        {
            return Results.Ok(await service.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(AuthorRequestDTO request, IDatabaseRepository<Author> service)
        {
            var target = await service.GetAll();

            //user exists
            if (target.Where(u => u.Email == request.Email).Any()) return Results.Conflict(new Payload<AuthorRequestDTO>() { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var author = new Author();

            author.Email = request.Email;
            author.Name = request.Name;
            author.PasswordHash = passwordHash;

            await service.Insert(author);
            await service.Save();

            return Results.Ok(new Payload<string>() { data = $"Created Account for Author with id! {author.Id}" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(AuthorLoginDTO request, IDatabaseRepository<Author> service, IConfigurationSettings config)
        {
            var target = await service.GetAll();

            //user doesn't exist
            if (!target.Where(u => u.Email == request.Email).Any()) return Results.BadRequest(new Payload<AuthorLoginDTO>() { status = "User does not exist!", data = request });

            Author author = target.FirstOrDefault(u => u.Email == request.Email)!;


            if (!BCrypt.Net.BCrypt.Verify(request.Password, author.PasswordHash))
            {
                return Results.BadRequest(new Payload<AuthorLoginDTO>() { status = "Wrong Password", data = request });
            }
            string token = CreateToken(author, config);
            return Results.Ok(new Payload<string>() { data = token });

        }
        private static string CreateToken(Author author, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, author.Email)
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
