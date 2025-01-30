using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Exceptions;
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
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User, int> repository, ClaimsPrincipal user)
        {
            return TypedResults.Ok(await repository.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(IRepository<User, int> repository, UserRegisterPost entity)
        {
            try
            {
                await repository.Find(x => x.UserName == entity.Username || x.Email == entity.Email);
                return Results.Conflict(new Payload { Status = "Username already exists!", Data = entity });
            }
            catch (EntityNotFoundException)
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(entity.Password);

                var user = new User
                {
                    UserName = entity.Username,
                    PasswordHash = passwordHash,
                    Email = entity.Email,
                };

                await repository.Add(user);

                return Results.Ok(new Payload { Data = "Created Account" });
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(IRepository<User, int> repository, IConfigurationSettings config, UserLoginPost entity)
        {
            User user;
            try
            {
                user = await repository.Find(x => x.Email == entity.Email);
            } catch (EntityNotFoundException)
            {
                return Results.BadRequest(new Payload { Status = "Incorrect information or user does not exist.", Data = entity });
            }
            //user doesn't exist

            if (!BCrypt.Net.BCrypt.Verify(entity.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload { Status = "Incorrect information or user does not exist.", Data = entity });
            }
            string token = CreateToken(user, config);
            return Results.Ok(new Payload { Data = token });

        }

        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
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
