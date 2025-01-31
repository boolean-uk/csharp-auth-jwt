using AutoMapper;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Endpoints
{
    public static class UserApi
    {
        public static void ConfigureUserApi(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> repo, IMapper mapper, ClaimsPrincipal user)
        {

            var users = repo.GetAll();
            var dto = mapper.Map<List<User>>(users);
            var payload = new Payload<List<User>>
            {
                Data = dto
            };
            return TypedResults.Ok(payload);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(IRepository<User> repo, IMapper mapper, RegisterUserDTO request)
        {
            var existingUser = await repo.GetAll();
            if (existingUser.FirstOrDefault(x => x.Username == request.Username) != null)
            {
                return TypedResults.Conflict("User already exists");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);
            var newUser = new User()
            {
                Username = request.Username,
                PasswordHash = request.PasswordHash
            };
            var result = await repo.Insert(newUser);
            var dto = mapper.Map<User>(result);
            var payload = new Payload<User>
            {
                Status = "User Created Successfully",
                Data = result
            };
            return TypedResults.Ok(payload);
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(LoginUserDTO request, IRepository<User> repo, IConfigurationSettings config)
        {
            //user doesn't exist

            var users = await repo.GetAll();
            var userExists = users.Where(u => u.Username == request.Username).Any();
            if (!userExists) return Results.BadRequest(new Payload<LoginUserDTO>() { Status = "User does not exist", Data = request });

            User user = users.FirstOrDefault(u => u.Username == request.Username)!;


            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<LoginUserDTO>() { Status = "Wrong Password", Data = request });
            }
            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string>() { Data = token });

        }
        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),

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