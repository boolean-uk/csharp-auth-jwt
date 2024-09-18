using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
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
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);

        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IDatabaseRepository<User> service)
        {
            //Response
            return Results.Ok(service.GetAll());
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDto request, IDatabaseRepository<User> service)
        {

            //Check if the user already exists
            if (service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequestDto>() { status = "Username already exists!", data = request });

            //Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            //Create a new user
            var user = new User();

            //Fill in the information
            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            //Put the user in the database
            service.Insert(user);
            service.Save();

            //Response
            return Results.Ok(new Payload<string>() { data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDto request, IDatabaseRepository<User> service, IConfigurationSettings config)
        {
            //Check if user doesn't exist
            if (!service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.BadRequest(new Payload<UserRequestDto>() { status = "User does not exist", data = request });

            //Get the user
            User user = service.GetAll().FirstOrDefault(u => u.Username == request.Username)!;
           
            //Verify that the correct password was used
            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserRequestDto>() { status = "Wrong Password", data = request });
            }

            //Create the JWT authorize token
            string token = CreateToken(user, config);

            //Response
            return Results.Ok(new Payload<string>() { data =  token }) ;
           
        }
        private static string CreateToken(User user, IConfigurationSettings config)
        {
            //Create a list of claims
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)                
            };
            
            //Create the key based on appsettings token information
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue("AppSettings:Token")));

            //Create the credentials by using HS512 on the key
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Create a 24 hour JWT token using the claims and credentials
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            //Response
            return jwt;
        }
    }
}

