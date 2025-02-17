using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Helper;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;


namespace exercise.wwwapi.Endpoints
{
    public static class AuthApi
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public static async Task<IResult> Register(UserRequestDTO request, IRepository<User> repository)
        {
            var users= await repository.GetAll();
            if (users.Any(u => u.Username == request.Username))
                return Results.Conflict(new Payload<UserRequestDTO> { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email
            };

            repository.Insert(user);
           
            return Results.Ok(new Payload<string> { data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Login(UserRequestDTO request, IRepository<User> repository, IConfigurationSettings config)
        {
            var users = await repository.GetAll();
            User? user = users.FirstOrDefault(u => u.Username == request.Username);
            if (user == null)
                return Results.BadRequest(new Payload<UserRequestDTO> { status = "User does not exist", data = request });

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Results.BadRequest(new Payload<UserRequestDTO> { status = "Wrong Password", data = request });

            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string> { data = token });
        }

        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email)
            };

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

