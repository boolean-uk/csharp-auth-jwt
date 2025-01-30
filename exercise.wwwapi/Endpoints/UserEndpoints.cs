using exercise.wwwapi.Models;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoints
    {
        public static void ConfigureUserEndpoints(this WebApplication app)
        {
            var userGroup = app.MapGroup("user");

            userGroup.MapPost("/", CreateUser);
            userGroup.MapGet("/", GetUsers);
            userGroup.MapGet("/{uuid}", GetUserByUuid);
            userGroup.MapPut("/{uuid}", UpdateUser);
            userGroup.MapDelete("/{uuid}", DeleteUser);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> CreateUser(IRepository<User> userRepository, string email, string password)
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
                HashedPassword = hashedPassword
            };

            var createdUser = await userRepository.Insert(user);

            return TypedResults.Created($"/user/{createdUser.Id}", new { status = "success", data = createdUser});
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetUsers(IRepository<User> userRepository)
        {
            var users = await userRepository.Get();
            return TypedResults.Ok(new { status = "success", data = users });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> GetUserByUuid(IRepository<User> userRepository, int id)
        {
            var user = await userRepository.GetById(id);
            if (user == null)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "User not found!" });
            }

            return TypedResults.Ok(new { status = "success", data = user});
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> UpdateUser(IRepository<User> userRepository, int id, string? email, string? password)
        {
            var user = await userRepository.GetById(id);
            if (user == null)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "User not found!" });
            }

            if (email != null)
            {
                user.Email = email;
            }
            if (password != null)
            {
                string hashedPassword = Util.HashPassword(password);
                user.HashedPassword = hashedPassword;
            }

            var updatedUser = await userRepository.Update(user);
            return TypedResults.Ok(new { status = "success", data = user });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> DeleteUser(IRepository<User> userRepository, int id)
        {
            var user = await userRepository.GetById(id);
            if (user == null)
            {
                return TypedResults.BadRequest(new { status = "failure", message = "User not found!" });
            }

            var deletedUser = await userRepository.Delete(user);

            return TypedResults.Ok(new { status = "success", data = deletedUser });
        }
    }
}
