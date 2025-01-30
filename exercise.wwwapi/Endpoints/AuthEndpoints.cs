using exercise.wwwapi.Models;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthEndpoints
    {
        public static void ConfigureAuthEndpoints(this WebApplication app)
        {
            var authGroup = app.MapGroup("auth");

            authGroup.MapPost("/register", Register);
            authGroup.MapPost("/login", Login);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> Register(IRepository<User> userRepository, string email, string password, string role = "User")
        {
            if (email == null || email == "")
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Email cannot be null or empty!" });
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Email has invalid format!" });
            }
            if (password == null || password == "")
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Password cannot be null or empty!" });
            }
            if (password.Length < 8)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Password has to be at least 8 characters long!" });
            }

            string hashedPassword = Util.HashPassword(password);

            var user = new User
            {
                Email = email,
                HashedPassword = hashedPassword,
                Role = role
            };

            var createdUser = await userRepository.Insert(user);

            return TypedResults.Ok(new { status = "success", data = createdUser });
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(IRepository<User> userRepository, IConfiguration config, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Email cannot be null or empty!" });
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Password cannot be null or empty!" });
            }

            var users = await userRepository.Get();
            var user = users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "User not found!" });
            }

            string hashedPassword = Util.HashPassword(password);

            if (hashedPassword != user.HashedPassword)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "Wrong password!" });
            }

            string token = Util.CreateToken(user, config);

            return TypedResults.Ok(new { status = "success", data = token });
        }
    }
}
