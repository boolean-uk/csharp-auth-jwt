using Azure.Core;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Endpoint
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("message", GetMessage).RequireAuthorization("AdminOrAuthor");
            app.MapGet("users", GetUsers).RequireAuthorization("Admin");
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetMessage([FromServices] IRepository<User> service, ClaimsPrincipal user, [FromServices] ILogger logger)
        {
            logger.LogDebug(new string('*', 1000));
            return TypedResults.Ok(new { LoggedIn = true, UserId = user.UserRealId().ToString(), Email = $"{user.Email()}", Message = "Pulled the userid and email out of the claims" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Admin")]
        private static async Task<IResult> GetUsers(IRepository<User> repository, ClaimsPrincipal user)
        {
            return TypedResults.Ok(repository.GetAll());
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register([FromBody] UserRequestDto request, [FromServices] IRepository<User> service, [FromServices] IUserRepository userRepository, ClaimsPrincipal user)
        {
            // Admin can only be assigned the "Admin" role by another admin
            if (request.IsAdmin && !user.IsInRole("Admin")) // Checks if the current user is not an Admin
                return Results.Forbid(); // Forbids if trying to register as Admin without permission

            // Check if the user already exists
            if (service.GetAll().Any(u => u.Username == request.Username))
                return Results.Conflict(new Payload<UserRequestDto>() { status = "Username already exists!", data = request });

            // Hash the password
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            // Create a new user object
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                Email = request.Email,
                Role = request.IsAdmin ? "Admin" : "Author"  // Set role to "Admin" if IsAdmin is true, otherwise set to "Author"
            };

            // Insert the new user into the repository
            await userRepository.AddUserAsync(newUser);

            return Results.Ok(new Payload<string>() { data = "Created Account" });
        }



        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login([FromBody] UserRequestDto request, [FromServices] IRepository<User> service, [FromServices] IConfigurationSettings config)
        {
            //user doesn't exist
            if (!service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.BadRequest(new Payload<UserRequestDto>() { status = "User does not exist", data = request });

            User user = service.GetAll().FirstOrDefault(u => u.Username == request.Username)!;


            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserRequestDto>() { status = "Wrong Password", data = request });
            }
            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string>() { data = token });

        }
        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)

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
