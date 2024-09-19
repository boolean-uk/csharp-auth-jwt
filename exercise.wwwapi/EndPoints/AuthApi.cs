using exercise.wwwapi.Configuration;
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

namespace exercise.wwwapi.EndPoints
{
    public static class AuthApi
    {
        public static void ConfigureAuthApi(this WebApplication app)
        {
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);
            app.MapPut("Follows/{id}", FollowUser);
            app.MapPut("Unfollow/{id}", UnfollowUser);
            app.MapGet("ViewAllFollowedPosts", GetFollowedPosts);

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
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
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

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> FollowUser(int followId, IDatabaseRepository<User> repository, ClaimsPrincipal user)
        {
            //Get current user ID
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //Check if both users exist
            var currentUser = repository.GetById(userId);
            var userToFollow = repository.GetById(followId);
            if (currentUser == null || userToFollow == null)
            {
                return Results.BadRequest();
            }

            //Check if the current user isn't already following the other user
            foreach(var follow in currentUser.Following)
            {
                if(follow.F_UserId == userToFollow.Id)
                {
                    return Results.BadRequest();
                }
            }

            //If no checks have failed, update the currentUser following list
            currentUser.Following.Add(new Follower() { Username = userToFollow.Username, F_UserId = userToFollow.Id });

            //Update the database
            repository.Update(currentUser);
            repository.Save();

            //Create the response
            var userResponse = new UserWithFollowingResponseDTO()
            {
                UserId = currentUser.Id,
                Username = currentUser.Username,
                PasswordHash = currentUser.PasswordHash
            };
            foreach (var fol in currentUser.Following)
            {
                userResponse.Following.Add(new FollowerDTO() { Id = fol.F_UserId, Username = fol.Username });
            }

            //Create payload
            var payload = new Payload<UserWithFollowingResponseDTO>()
            {
                data = userResponse
            };

            //Response
            return Results.Created($"https://localhost:5005/users/{payload.data.UserId}", payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> UnfollowUser(int followId, IDatabaseRepository<User> repository, ClaimsPrincipal user)
        {
            //Get current user ID
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //Check if both users exist
            var currentUser = repository.GetById(userId);
            var userToFollow = repository.GetById(followId);
            if (currentUser == null || userToFollow == null)
            {
                return Results.BadRequest();
            }

            //Check if the current user is already following the other user
            var follow = currentUser.Following.Where(f => f.F_UserId == userToFollow.Id).FirstOrDefault();
            if (follow == null)
            {
                return Results.BadRequest();
            }

            //If no checks have failed, update the currentUser following list
            currentUser.Following.Remove(follow);

            //Update the database
            repository.Update(currentUser);
            repository.Save();

            //Create the response
            var userResponse = new UserWithFollowingResponseDTO()
            {
                UserId = currentUser.Id,
                Username = currentUser.Username,
                PasswordHash = currentUser.PasswordHash
            };
            foreach (var fol in currentUser.Following)
            {
                userResponse.Following.Add(new FollowerDTO() { Id = fol.F_UserId, Username = fol.Username });
            }

            //Create payload
            var payload = new Payload<UserWithFollowingResponseDTO>()
            {
                data = userResponse
            };

            //Response
            return Results.Created($"https://localhost:5005/users/{payload.data.UserId}", payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetFollowedPosts(IDatabaseRepository<User> userRepository, IDatabaseRepository<BlogPost> blogRepository, ClaimsPrincipal user)
        {
            //Get current user ID
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            //Get all followed IDs
            List<int> followedIds = new List<int>();

            var following = userRepository.GetById(userId).Following;

            foreach (var follow in following)
            {
                followedIds.Add(follow.F_UserId);
            }

            //Get all posts
            var blogposts = blogRepository.GetAll().Where(b => followedIds.Contains(b.UserId));

            //Create responses
            List<BlogWithCommentsResponseDTO> result = new List<BlogWithCommentsResponseDTO>();
            foreach (var blogpost in blogposts)
            {
                var addition = new BlogWithCommentsResponseDTO() { PostId = blogpost.Id, Username = blogpost.User.Username, Post = blogpost.Post };
                foreach (var com in blogpost.Comments)
                {
                    addition.Comments.Add(new CommentDTO() { Username = com.Username, Text = com.Text });
                }
                result.Add(addition);
            }

            //Create payload
            var payload = new Payload<List<BlogWithCommentsResponseDTO>>()
            {
                data = result
            };

            return Results.Ok(payload);
        }
    }
}

