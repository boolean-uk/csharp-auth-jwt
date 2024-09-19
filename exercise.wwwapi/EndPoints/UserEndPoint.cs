using exercise.wwwapi.Configuration;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.EndPoints
{
    public static class UserEndPoint
    {
        public static void UserEndpointConfiguration(this WebApplication app)
        {
            var users = app.MapGroup("users");
            users.MapPost("/", Register);
            users.MapGet("/", GetUsers);
            users.MapPost("/login", Login);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> GetUsers(IRepository<User> repository, ClaimsPrincipal user)
        {
            return TypedResults.Ok(await repository.GetAll());
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> Register(IRepository<User> repository, UserRequest User)
        {
            IEnumerable<User> users = await repository.GetAll();
            if (users.Where(u => u.Username == User.UserName).Any()) return Results.Conflict(new Payload<UserRequest>() { status = "Username taken", data = User });
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(User.Password);
            User Dbuser = new User();

            Dbuser.Username = User.UserName;
            Dbuser.PasswordHash = passwordHash;
            
            await repository.Add(Dbuser);
            return TypedResults.Ok(new Payload<string>() { data = "Account Created"});
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(IRepository<User> repository, UserRequest user, IConfigurationSettings config)
        {
            IEnumerable<User> users = await repository.GetAll();
            if (!users.Where(u => u.Username == user.UserName).Any()) return Results.Conflict(new Payload<UserRequest>() { status = "Username not found", data = user });
            User dbUser = users.FirstOrDefault(u => u.Username == user.UserName)!;

            if(!BCrypt.Net.BCrypt.Verify(user.Password, dbUser.PasswordHash))
            {
                return TypedResults.BadRequest(new Payload<UserRequest>() {status = "Wrong Password", data = user });
            }
            string token = CreateToken(dbUser, config);
            return TypedResults.Ok(token);
        }
        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            { 
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue("Apptoken:Token")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMinutes(5)
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
