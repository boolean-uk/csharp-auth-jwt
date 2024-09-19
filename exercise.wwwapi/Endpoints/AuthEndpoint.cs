﻿using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthEndpoint
    {
        public static void AuthEndpointConfiguration(this WebApplication app)
        {
            var authentications = app.MapGroup("auth");

            app.MapPost("/register", Register);
            app.MapPost("/login", Login);
            app.MapGet("/users", GetUsers);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> service)
        {
            List<UserResponseDTO> listResultDTO = new List<UserResponseDTO>();
            
            foreach (var result in await service.GetAll())
            {
                listResultDTO.Add(new UserResponseDTO(result));
            }

            var payload = new Payload<List<UserResponseDTO>>() { data = listResultDTO };
            return TypedResults.Ok(payload);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDTO request, IRepository<User> service)
        {

            //user exists
            var result = await service.GetAll(u => u.Username == request.Username);
            if (result.Any()) return Results.Conflict(new Payload<UserRequestDTO>() { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User();

            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            await service.Create(user);

            return Results.Ok(new Payload<string>() { data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDTO request, IRepository<User> service, IConfigurationSettings config)
        {
            //user doesn't exist
            var result = await service.GetAll(u => u.Username == request.Username);
            if (!result.Any()) return Results.BadRequest(new Payload<UserRequestDTO>() { status = "User does not exist", data = request });

            User user = result.FirstOrDefault(u => u.Username == request.Username);

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
