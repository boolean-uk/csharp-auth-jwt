using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.UserDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.EndPoints
{
    public static class AuthorEndpoint
    {
        public static void ConfigureAuthorEndpoint(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequest request, IRepository<User> repository)
        {

            //user exists
            if (repository.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequest>() { status = "Username already exists!", Data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User();

            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            repository.Add(user);
            repository.Save();

            return Results.Ok(new Payload<string>() { Data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequest request, IRepository<User> repository, IConfigurationSettings config)
        {
            //user doesn't exist
            if (!repository.GetAll().Where(u => u.Username == request.Username).Any()) return Results.BadRequest(new Payload<UserRequest>() { status = "User does not exist", Data = request });

            User user = repository.GetAll().FirstOrDefault(u => u.Username == request.Username)!;


            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserRequest>() { status = "Wrong Password", Data = request });
            }
            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string>() { Data = token });

        }

        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)     // collection of claims
                                                            
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue("AppSettings:Token")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature); // security algorithm
            var token = new JwtSecurityToken( // Generating token. Pass in out claim
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}

