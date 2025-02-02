
using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Models;
using exercise.wwwapi.Payload;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        blog.MapGet("/all", GetBlogs);
        blog.MapPost("/create", CreateBlog);
        blog.MapPut("/{id}", UpdateBlog);

        // Blog endpoints

        
    }


    #region User endpoints

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
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

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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

    [Authorize]
    private static async Task<IResult> GetUsers(IRepository<User> repository, ClaimsPrincipal user, IMapper mapper)
    {
        var users = await repository.GetAll();
        Payload<IEnumerable<GetUserDTO>> payload = new Payload<IEnumerable<GetUserDTO>> {Data = mapper.Map<IEnumerable<GetUserDTO>>(users)};
        return Results.Ok(payload);
    }
    #endregion

    #region Blog endpoints
    [Authorize]
    public static async Task<IResult> GetBlogs(IRepository<Blog> Blogrepository, IRepository<User> Userrepository, ClaimsPrincipal user, IMapper mapper)
    {
        int userId = user.UserRealId() != null ? user.UserRealId()!.Value : 0;
        User usr = await Userrepository.GetEntityById(userId);

        Payload<IEnumerable<GetBlogDTO>> payload = new Payload<IEnumerable<GetBlogDTO>> {Data = mapper.Map<List<GetBlogDTO>>(usr.Blogs)};
        return Results.Ok(payload);
    }

    public static async Task<IResult> CreateBlog(IRepository<Blog> repository, ClaimsPrincipal user, IMapper mapper, CreateBlogDTO blogDTO)
    {
        int userId = user.UserRealId() != null ? user.UserRealId()!.Value : 0;
        if (userId == 0) return Results.Unauthorized();

        Blog blog = mapper.Map<Blog>(blogDTO);
        blog.UserId = userId;
        Blog newBlog = await repository.CreateEntity(blog);
        Payload<GetBlogDTO> payload = new Payload<GetBlogDTO> {Data = mapper.Map<GetBlogDTO>(newBlog)};
        return Results.Ok(payload);
    }

    public static async Task<IResult> UpdateBlog(IRepository<Blog> blogrepository, IRepository<User> userrepository, ClaimsPrincipal user, IMapper mapper, int id, UpdateBlogDTO blogDTO)
    {
        int userId = user.UserRealId() != null ? user.UserRealId()!.Value : 0;
        if (userId == 0) return Results.Unauthorized();

        User usr = await userrepository.GetEntityById(userId);
        Blog currentBlog = await blogrepository.GetEntityById(id);
        if (currentBlog.UserId != userId) return Results.Unauthorized();


        Blog updatedBlog = await blogrepository.UpdateEntityById(id, mapper.Map<Blog>(blogDTO));
        Payload<GetBlogDTO> payload = new Payload<GetBlogDTO> {Data =mapper.Map<GetBlogDTO>(updatedBlog)};
        return Results.Ok(payload);
    }
  

    #endregion
}
