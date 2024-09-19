using exercise.wwwapi.DTOs;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class UserEndpoint
    {
        private static string _path = AppContext.BaseDirectory;
        public static void UserEndpointConfiguration(this WebApplication app)
        {
            var users = app.MapGroup("user");
            users.MapGet("/follow/{id}", FollowUser);
            users.MapPost("/unfollow/{id}", UnFollowUser);
            users.MapPut("/viewall/", ViewAllFollowingUserPosts);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> FollowUser(IRepository<User> userRepository, IRepository<UserFollow> userFollowRepository, int id, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            // Checks if user already is followed
            var followerUser = await userRepository.Get(x => x.Id == userId);
            if (followerUser.Following.Where(uf => uf.FollowedId == id).Any())
            {
                return Results.BadRequest("Already following this user");
            }

            // Creating new user follow relation and 
            await userFollowRepository.Create(new UserFollow()
            {
                FollowedId = id,
                FollowerId = (int)userId
            });

            var updatedUser = await userRepository.GetAll();

            var resultDTO = new UserResponseDTO(updatedUser.FirstOrDefault(x => x.Id == userId));

            var payload = new Payload<UserResponseDTO>() { data = resultDTO };
            return TypedResults.Created(_path, payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> UnFollowUser(IRepository<User> userRepository, IRepository<UserFollow> userFollowRepository, int id, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            // Checks if user already is not followed
            var followerUser = await userRepository.Get(x => x.Id == userId);
            if (!followerUser.Following.Where(uf => uf.FollowedId == id).Any())
            {
                return Results.BadRequest("Already not following this user");
            }

            var followedUser = await userRepository.Get(x => x.Id == userId);

            // Removing user follow relation 
            var userFollow = await userFollowRepository.Get(x => x.FollowerId == followerUser.Id && x.FollowedId == followedUser.Id);
            await userFollowRepository.Delete(userFollow);

            var updatedUser = await userRepository.GetAll();

            var resultDTO = new UserResponseDTO(updatedUser.FirstOrDefault(x => x.Id == userId));

            var payload = new Payload<UserResponseDTO>() { data = resultDTO };
            return TypedResults.Created(_path, payload);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public static async Task<IResult> ViewAllFollowingUserPosts(IRepository<User> userRepository, IRepository<BlogPost> blogPostRepository, ClaimsPrincipal user)
        {
            //Check if the user is logged in
            var userId = user.UserRealId();
            if (userId == null)
            {
                return Results.Unauthorized();
            }

            var targetUser = await userRepository.Get(x => x.Id == userId);

            List<AuthorBlogPostsResponseDTO> resultDTOs = new List<AuthorBlogPostsResponseDTO>();

            foreach (User followedUser in targetUser.Following.Select(uf => uf.Followed).ToList())
            {
                AuthorBlogPostsResponseDTO authorBlogPostsResponseDTO = 
                    new AuthorBlogPostsResponseDTO(followedUser);

                var blogPosts = await blogPostRepository.GetAll(bp => bp.AuthorId == followedUser.Id);

                foreach (var blogPost in blogPosts)
                {
                    authorBlogPostsResponseDTO.blogPosts.Add(new BlogPostResponseDTOAuthorLess(blogPost));
                }
                resultDTOs.Add(authorBlogPostsResponseDTO);
            }

            var payload = new Payload<List<AuthorBlogPostsResponseDTO>>() { data = resultDTOs };
            return TypedResults.Ok(payload);
        }
    }
}
