using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTOs;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;

namespace exercise.wwwapi.Endpoints
{
    public static class BlogApis
    {
        public static void ConfigureBlogApis(this WebApplication app)
        {
            app.MapGroup("blogPost");
            app.MapPost("login", Login);
            app.MapPost("register", Register);
            app.MapGet("/posts", GetPosts);
            app.MapPost("/posts", CreatePost);
            app.MapPut("posts", EditPost);
            app.MapPost("follow", FollowUser);
            app.MapPost("unfollow", UnfollowUser);
            app.MapGet("viewAllPosts", ViewallPosts);
        }

        private static string CreateToken(Author author, IConfigurationSettings config)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, author.userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.getValue("AppSettings:Token")));
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetPosts(IRepository<BlogPost> service)
        {

            return Results.Ok(service.GetAll());
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> CreatePost(IRepository<BlogPost> repository, CreateBlogPostDTO dto, HttpContext httpContext)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Results.BadRequest(new Payload<CreateBlogPostDTO> { status = "user does not exist", data = dto });
            }
            var userNameClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (userNameClaim == null)
            {
                return Results.BadRequest(new { message = "Username claim not found in token" });
            }
            string username = userNameClaim.Value;
            var author = repository.GetAll().FirstOrDefault(a => a.author.userName == username);
            BlogPost blog = new BlogPost()
            {

                authorId = dto.authorId,
                text = dto.text,



            };

            return Results.Ok(blog);

        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(RegisterDTO request, IRepository<Author> service)
        {

            if (service.GetAll().Where(u => u.userName == request.userName).Any()) return Results.Conflict(new Payload<RegisterDTO>() { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var author = new Author();
            author.Id = Guid.NewGuid().ToString();
            author.userName = request.userName;
            author.passwordHash = passwordHash;
            author.Name = request.Name;


            service.Insert(author);
            service.Save();

            return Results.Ok(new Payload<string>() { data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(loginDTO loginRequest, IRepository<Author> service, IConfigurationSettings config)
        {
            if (!service.GetAll().Where(u => u.userName == loginRequest.username).Any()) return Results.BadRequest(new Payload<loginDTO>() { status = "User does not exist", data = loginRequest });

            Author author = service.GetAll().FirstOrDefault(u => u.userName == loginRequest.username)!;


            if (!BCrypt.Net.BCrypt.Verify(loginRequest.password, author.passwordHash))
            {
                return Results.BadRequest(new Payload<loginDTO>() { status = "Wrong Password", data = loginRequest });
            }
            string token = CreateToken(author, config);
            return Results.Ok(new Payload<string>() { data = token });

        }

        public static async Task<IResult> EditPost(IRepository<BlogPost> repository, IRepository<Author> authorService, CreateBlogPostDTO dto, HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            if (identity == null)
            {
                return Results.BadRequest(new Payload<CreateBlogPostDTO> { status = "user does not exist", data = dto });
            }
            var userNameClaim = identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (userNameClaim == null)
            {
                return Results.BadRequest(new { message = "Username claim not found in token" });
            }
            string username = userNameClaim.Value;
            Author author = authorService.GetById(dto.authorId);
            if (author.Id == dto.authorId)
            {
                BlogPost blog = new BlogPost()
                {
                    authorId = dto.authorId,
                    text = dto.text,
                };
                repository.Update(blog);
                return Results.Ok(new Payload<CreateBlogPostDTO> { status = "blog edited successfully", data = dto });
            }
            else
            {
                return Results.BadRequest(new { message = "You are not the author of this blog" });
            }
        }


        public static async Task<IResult> FollowUser(string followerId, string followedId, IRepository<AuthorFollower> followerService, IRepository<Author> authorService)
        {
            var follower = authorService.GetById(followerId);
            var followed = authorService.GetById(followedId);

            if (follower == null || followed == null)
            {
                return Results.NotFound(new { message = "One or both users not found" });
            }

            var existingFollow = followerService.GetAll().FirstOrDefault(af => af.FollowerId == followerId && af.FollowedId == followedId);
            if (existingFollow != null)
            {
                return Results.Conflict(new { message = "User is already following this user" });
            }

            var follow = new AuthorFollower
            {
                FollowerId = followerId,
                FollowedId = followedId
            };

            followerService.Insert(follow);
            followerService.Save();

            return Results.Ok(new { message = "User successfully followed" });
        }

        public static async Task<IResult> UnfollowUser(string followerId, string followedId, IRepository<AuthorFollower> followerService, IRepository<Author> authorService)
        {
            var follower = authorService.GetById(followerId);
            var followed = authorService.GetById(followedId);

            if (follower == null || followed == null)
            {
                return Results.NotFound(new { message = "One or both users not found" });
            }

            var existingFollow = followerService.GetAll().FirstOrDefault(af => af.FollowerId == followerId && af.FollowedId == followedId);
            if (existingFollow == null)
            {
                return Results.NotFound(new { message = "User is not following this user" });
            }


            followerService.Delete(existingFollow);
            followerService.Save();

            return Results.Ok(new { message = "User successfully unfollowed" });
        }


        public static async Task<IResult> ViewallPosts(string userId, IRepository<AuthorFollower> followerService, IRepository<BlogPost> blogPostService)
        {
            var followedUsersIds = followerService.GetAll()
                .Where(af => af.FollowerId == userId)
                .Select(af => af.FollowedId)
                .ToList();

            // Fetch blog posts of followed users asynchronously
            var blogPosts = blogPostService.GetAll()
                .Where(bp => followedUsersIds.Contains(bp.authorId))
                .ToList();

            if (!blogPosts.Any())
            {
                return Results.NotFound(new { message = "No posts found for followed users" });
            }

            return Results.Ok(blogPosts);
        }
    }
}

