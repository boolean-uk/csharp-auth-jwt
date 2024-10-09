using exercise.wwwapi.Configuration;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Models.DTOs;
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
            var authentications = app.MapGroup("auth");

            authentications.MapPost("register", Register);
            authentications.MapPost("login", Login);
            authentications.MapGet("users", GetUsers);

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> repository, ClaimsPrincipal user)
        {
            var users = await repository.GetAll();
            return TypedResults.Ok(users);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDTO request, IRepository<User> repository)
        {
            // Check if user exists
            var userExists = (await repository.GetAll(u => u.Username == request.Username)).Any();
            if (userExists)
                return Results.Conflict(new Payload<UserRequestDTO> { Status = "Username already exists!", Data = request });

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create new user
            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash
            };

            // Insert the new user and save changes
            await repository.Create(user);
            repository.Save();

            return Results.Ok(new Payload<string> { Data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDTO request, IRepository<User> repository, IConfigurationSettings config)
        {
            // Check if user exists
            var user = (await repository.GetAll(u => u.Username == request.Username)).FirstOrDefault();
            if (user == null)
                return Results.BadRequest(new Payload<UserRequestDTO> { Status = "User does not exist", Data = request });

            // Verify the password
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserRequestDTO> { Status = "Wrong Password", Data = request });
            }

            // Create and return the JWT token
            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string> { Data = token });
        }

        private static string CreateToken(User user, IConfigurationSettings config)
        {
            // Create claims for the token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Generate the JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue("AppSettings:Token")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
