using exercise.wwwapi.Configuration;
using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.Extensions;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthAPI
    {
        public static void ConfigureAuthAPI(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IDatabaseRepository<User> repository)
        {
            var users = await repository.GetAll();
            List<UserDTO> userDTOs = (from user in users select user.ToDTO()).ToList();
            var payload = new Payload<IEnumerable<UserDTO>>() { Status = "success", Data = userDTOs};
            return TypedResults.Ok(payload);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserCreate userPost, IDatabaseRepository<User> service)
        {
            var users = await service.GetAll();
            //user exists
            if (users.Where(u => u.Username == userPost.Username).Any()) return TypedResults.Conflict(new Payload<UserCreate>() { Status = "Username already exists!", Data = userPost });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(userPost.Password);

            var user = new User();

            user.Username = userPost.Username;
            user.PasswordHash = passwordHash;

            await service.Insert(user);

            return TypedResults.Ok(new Payload<string>() {Status = "success", Data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserCreate userPost, IDatabaseRepository<User> service, IConfigurationSettings config)
        {
            var users = await service.GetAll();
            //user doesn't exist
            if (!users.Where(u => u.Username == userPost.Username).Any()) return TypedResults.BadRequest(new Payload<UserCreate>() { Status = "User does not exist", Data = userPost });

            User user = users.FirstOrDefault(u => u.Username == userPost.Username)!;


            if (!BCrypt.Net.BCrypt.Verify(userPost.Password, user.PasswordHash))
            {
                return TypedResults.BadRequest(new Payload<UserCreate>() { Status = "Wrong Password", Data = userPost });
            }
            string token = CreateToken(user, config);
            return TypedResults.Ok(new Payload<string>() { Status = "success", Data = token });

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
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
