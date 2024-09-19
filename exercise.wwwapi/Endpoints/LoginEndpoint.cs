using exercise.wwwapi.Configuration;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransferObjects;
using exercise.wwwapi.DataViews;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Endpoints
{
    public static class LoginEndpoint
    {
        public static void ConfigureLoginEndpoint(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(IRepository<User> service, UserView request)
        {
            // check if user with sent in username exists
            var userRequest = await service.Get(u => u.Username == request.Username);
            if (userRequest != null) return Results.Conflict(new Payload<UserView>() { Status = "Username already exists!", Data = request });

            // hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User()
            {
                Username = request.Username,
                PasswordHash = passwordHash,
            };

            await service.Create(user);

            return Results.Ok(new Payload<string>() { Data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(IRepository<User> service, UserView request, IConfigurationSettings config)
        {
            // get user and check if exists
            var user = await service.Get(u => u.Username == request.Username);
            if (user == null) return Results.BadRequest(new Payload<UserView>() { Status = "User does not exist", Data = request });

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserView>() {Status = "Wrong Password", Data = request });
            }

            // create a bearer token and return it
            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string>() { Data = token });

        }
        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString())
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
