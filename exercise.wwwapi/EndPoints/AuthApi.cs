﻿using AutoMapper;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
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

        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> service, IMapper mapper, ClaimsPrincipal user)
        {
            var users = mapper.Map<List<UserResponseDTO>>(service.GetAll());
            return TypedResults.Ok(new PayloadCollection<UserResponseDTO>() { Data = users });
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDTO request, IRepository<User> service)
        {

            //user exists
            if (service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequestDTO>() { Status = "Username already exists!", Data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User();

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.Email = request.Email;

            service.Insert(user);
            service.Save();

            return Results.Ok(new Payload<string>() { Data = "Created Account" });
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDTO request, IRepository<User> service, IConfigurationSettings config)
        {
            //user doesn't exist
            if (!service.GetAll().Where(u => u.Username == request.Username)
                .Any()) return Results.BadRequest(new Payload<UserRequestDTO>() { Status = "User does not exist", Data = request });

            User user = service.GetAll().FirstOrDefault(u => u.Username == request.Username)!;
           

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.BadRequest(new Payload<UserRequestDTO>() { Status = "Wrong Password", Data = request });
            }
            string token = CreateToken(user, config);
            return Results.Ok(new Payload<string>() { Data =  token }) ;
           
        }
        private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                
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

