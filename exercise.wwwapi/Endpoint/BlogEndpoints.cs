
using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Payload;
using exercise.wwwapi.Repository;
using Microsoft.IdentityModel.Tokens;

namespace exercise.wwwapi.Endpoint;

public static class BlogEndpoints
{
    
    public static void ConfigureBlogEndpoints(this WebApplication app)
    {
        var blog = app.MapGroup("/blogs");

        // User endpoints
        blog.MapPost("/register", Register);
        blog.MapPost("/login", LogIn);
        blog.MapGet("/users", GetUsers);

        // Blog endpoints

        
    }


    #region User endpoints

    public static async Task<IResult> Register(IRepository<User> repository, CreateUserDTO userDTO, IMapper mapper)
    {
        Func<User, bool> email_exists = u => u.Email == userDTO.Email;

        if (repository.Exists(email_exists))
            return Results.Conflict("Email already exists");

        string passwordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
        User user = mapper.Map<User>(userDTO);
        user.Password = passwordHash;
        var newUser = await repository.CreateEntity(user);
        Payload<GetUserDTO> payload = new Payload<GetUserDTO> {Data = mapper.Map<GetUserDTO>(newUser)};
        return Results.Ok(payload);
    }


    public static async Task<IResult> LogIn(IRepository<User> repository, IConfigurationSettings config, LogInDTO logInDTO, IMapper mapper)
    {
        User user = (await repository.GetAll()).ToList().FirstOrDefault(u => u.Email == logInDTO.Email)!;
        
        if (user == null)
            return Results.NotFound("User not found");

        if (!BCrypt.Net.BCrypt.Verify(logInDTO.Password, user.Password))
            return Results.Unauthorized();

        Payload<string> payload = new Payload<string> {Data = CreateToken(user, config)};
        return Results.Ok(payload);
    }

    private static string CreateToken(User user, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
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


    private static async Task<IResult> GetUsers(IRepository<User> repository, IMapper mapper)
    {
        var users = await repository.GetAll();
        Payload<IEnumerable<GetUserDTO>> payload = new Payload<IEnumerable<GetUserDTO>> {Data = mapper.Map<IEnumerable<GetUserDTO>>(users)};
        return Results.Ok(payload);
    }
    #endregion

    #region Blog endpoints


    #endregion
}
