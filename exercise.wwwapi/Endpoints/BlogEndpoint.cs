﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogEndpoint
    {
        public static void ConfigureBlogEndpoint(this WebApplication app)
        {
            app.MapPost("/register", Register);
            app.MapPost("/login", Login);

            app.MapGet("/blogposts", GetBlogPosts);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetBlogPosts(IPostRepository repository, ClaimsPrincipal user)
        {
            var posts = await repository.GetAll();
            Payload<List<PostDTO>> payload = new Payload<List<PostDTO>>() { data = new List<PostDTO>() };
            foreach (var post in posts)
            {
                payload.data.Add(new PostDTO
                {
                    ID = post.Id,
                    Author = post.Author.Username,
                    Text = post.Text
                });
            }
            return TypedResults.Ok(payload);
        }

        /*public static async Task<BlogPost> PostToBlog(IPostRepository repo, ClaimsPrincipal user)
        {
        }*/


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDTO request, IUserRepository service)
        {

            //user exists
            if (service.GetUserByUsername(request.Username) == null)
            {
                return Results.Conflict(new Payload<UserRequestDTO>() { status = "Username not available", data = request });
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await service.Add( new User() { Username = request.Username, PasswordHash = passwordHash });

            return Results.Created("", new Payload<string>() { data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDTO request, IUserRepository service, IConfigurationSettings config)
        {
            User user = await service.GetUserByUsername(request.Username);
            if (user == null)
            {
                return Results.BadRequest(new Payload<UserRequestDTO>() { status = "User does not exist", data = request });
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserRequestDTO>() { status = "Wrong Password", data = request });
            }

            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string>() { data = token });

        }

        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
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
