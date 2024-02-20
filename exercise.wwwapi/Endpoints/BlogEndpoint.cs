using exercise.wwwapi.DataModels.DTOs;
using exercise.wwwapi.DataModels.Models;
using exercise.wwwapi.DataTransfer.Requests.BlogPost;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoint 
    {
        public static void ConfigureBlogEndpoint(this WebApplication app)
        {
            var blogs = app.MapGroup("posts");
            blogs.MapGet("/", GetAllBlogs);
            blogs.MapPost("/", PostBlog);
            blogs.MapPut("/{id}", UpdateBlog);
            //blogs.MapDelete("/{id}", DeleteBlog);
        }

        /// <summary>
        /// Gets all the blogs. You have to be logged in to see the blogs.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userRepo"></param>
        /// <returns>A list of all the blogs in a DTO format</returns>
        [Authorize(Roles = "Admin,User")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetAllBlogs(IRepository<BlogPost> repository, ApplicationUserRepository userRepo)
        {
            var entities = await repository.SelectAll();
            var dtos = entities.Select(e => EntityConverter.EntityMapper<BlogPostDTO>(e, new List<string> { "User" })).ToList();
            //Awful code hahaha
            foreach (var dto in dtos)
            {
                dto.User = EntityConverter.EntityMapper<ApplicationUserDTO>(await userRepo.SelectById(dto.UserId), new List<string> { "Role" });
                dto.User.Role = (await userRepo.SelectById(dto.UserId)).Role.ToString();
            }
            if (dtos == null)
            {
                return Results.NotFound("No blogs are found");
            }
            return Results.Ok(dtos);
        }

        /// <summary>
        /// Adds a blog to the database. You have to be logged in to add a blog.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userRepo"></param>
        /// <param name="user"></param>
        /// <param name="blogPostPost"></param>
        /// <returns>Returns a DTO of the blogpost with a DTO of the user who created it inside</returns>
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> PostBlog(IRepository<BlogPost> repository, ApplicationUserRepository userRepo, ClaimsPrincipal user, BlogPostPostRequest blogPostPost)
        {
            var currentUser = await userRepo.SelectByEmail(user.Email()); ;
            if (currentUser == null)
            {
                return TypedResults.NotFound("User could not be found?");
            }

            // Create a new instance of BlogPost and populate its properties
            var blogPost = new BlogPost
            {
                Title = blogPostPost.Title,
                Content = blogPostPost.Content,
                UserId = currentUser.Id // Associate the user's email with the blog post
            };

            // Add the blog post to the repository
            var result = await repository.Insert(blogPost);
            if (result == null)
            {
                return Results.BadRequest("Failed to create blog post");
            }

            // Map the created blog post to a DTO if necessary
            var createdBlogPostDto = EntityConverter.EntityMapper<BlogPostDTO>(result, new List<string> { "User" });
            createdBlogPostDto.User = EntityConverter.EntityMapper<ApplicationUserDTO>(currentUser, new List<string> { "Role" });
            createdBlogPostDto.User.Role = currentUser.Role.ToString();
            createdBlogPostDto.UserId = currentUser.Id;

            // Return the created blog post
            return Results.Created($"/blogs/{createdBlogPostDto.Id}", createdBlogPostDto);
        }

        /// <summary>
        /// Updates a blog post. You have to be logged in to update a blog post.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userRepo"></param>
        /// <param name="user"></param>
        /// <param name="id"></param>
        /// <param name="blogPostPut"></param>
        /// <returns>A DTO of the updated blog with the original user in a DTO format as well</returns>
        [Authorize(Roles = "Admin,User")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> UpdateBlog(IRepository<BlogPost> repository, ApplicationUserRepository userRepo, ClaimsPrincipal user, int id, BlogPostPatchRequest blogPostPut)
        {
            var blogPost = await repository.SelectById(id);
            if (blogPost == null)
            {
                return Results.NotFound("Blog post not found");
            }

            var currentUser = await userRepo.SelectByEmail(user.Email());
            if (currentUser == null)
            {
                return Results.NotFound("User not found");
            }

            if (currentUser.Id != blogPost.UserId && !user.IsInRole("Admin"))
            {
                return Results.Unauthorized();
            }

            blogPost.Title = blogPostPut.Title;
            blogPost.Content = blogPostPut.Content;

            var result = await repository.Update(blogPost);
            if (result == null)
            {
                return Results.BadRequest("Failed to update blog post");
            }

            var createdBlogPostDto = EntityConverter.EntityMapper<BlogPostDTO>(result, new List<string> { "User" });
            var userWhoCreatedTheBlog = await userRepo.SelectById(blogPost.UserId);
            createdBlogPostDto.User = EntityConverter.EntityMapper<ApplicationUserDTO>(userWhoCreatedTheBlog, new List<string> { "Role" });
            createdBlogPostDto.User.Role = userWhoCreatedTheBlog.Role.ToString();
            createdBlogPostDto.UserId = userWhoCreatedTheBlog.Id;

            return Results.Ok(createdBlogPostDto);
        }
    }
}
