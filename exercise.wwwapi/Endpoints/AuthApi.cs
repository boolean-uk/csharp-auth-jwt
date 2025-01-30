using System.Security.Claims;
using exercise.wwwapi.Repository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.DTO;
using System.Runtime.CompilerServices;
using exercise.wwwapi.Helper;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthApi
    {

        public static void ConfigureAuthApi(this WebApplication app)
        {

            app.MapGet("/posts", GetPosts);
            app.MapPost("/posts", CreatePost);
            app.MapPut("/posts", EditPost);
            app.MapPost("register", Register);
            app.MapPost("login", Login);
            app.MapGet("users", GetUsers);
            app.MapPost("/posts/comment", CreateComment);
            app.MapGet("/postswithcomments", GetPostComments);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        private static async Task<IResult> GetUsers(IRepository<User> service, ClaimsPrincipal user)
        {
            return TypedResults.Ok(service.GetAll());
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetPosts(IRepository<Post> repository, IRepository<Comment> commentRepo, IRepository<User> userRepo,ClaimsPrincipal user)
        {
            
            AllPostCommentsDTO allpostcomments = new AllPostCommentsDTO();
           
            foreach(Post post in repository.GetAll())
            {
                PostWithCommentDTO postwithcomment = new PostWithCommentDTO();
                postwithcomment.postUsername = userRepo.GetById(int.Parse(post.AuthorId)).Username;
                postwithcomment.postText = post.Text;
                postwithcomment.PostId = post.Id;

                foreach (Comment comment in commentRepo.GetAll())
                {
                    
                    if (post.Id == comment.PostId)
                    {
                        
                        postwithcomment.comments.Add(new CommentWithUser() { comment = comment.Text, username = userRepo.GetById(comment.UserId).Username });
                    }
                }
                allpostcomments.Posts.Add(postwithcomment);
                
            }
            return TypedResults.Ok(allpostcomments);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreatePost(IRepository<Post> repository, IRepository<User> userRepo, PostRequestDTO postDTO, ClaimsPrincipal user)
        {
            User userr = userRepo.GetById(user.UserRealId());
            Post post = new Post()
            {
                Text = postDTO.Text,
                AuthorId = userr.Id.ToString()
            };


            repository.Insert(post);
            repository.Save();



            return TypedResults.Ok(new Payload<string>() { data = $"{postDTO.Text}" });
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> CreateComment(ClaimsPrincipal user, IRepository<Post> repository, IRepository<User> userRepo, IRepository<Comment> commentRepo, int postId, PostRequestDTO postCommentDTO)
        {
            

            Post post = repository.GetById(postId);


            User userr = userRepo.GetById(user.UserRealId());
            
            
            Comment comment = new Comment() { PostId = post.Id , UserId = userr.Id, Text = postCommentDTO.Text };
            
            commentRepo.Insert(comment);
            commentRepo.Save();


            
            return TypedResults.Ok(new Payload<string>() { data = $"{userr.Username} commented: {comment.Text}" });
        }


        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> EditPost(IRepository<Post> repository, IRepository<User> userRepo, ClaimsPrincipal userr, PostRequestDTO postDTO, int PostId)
        {
            Post post = repository.GetAll().Where(x => x.Id == PostId).FirstOrDefault();

            User user = userRepo.GetById(int.Parse(post.AuthorId));

            if (user.Id != userr.UserRealId())
            {
                return TypedResults.BadRequest("You cant change someone elses post");
            }

            post.Text = postDTO.Text;

            repository.Update(post);
            repository.Save();

            return TypedResults.Ok(new Payload<string>() { data = $"{postDTO.Text}" });

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public static async Task<IResult> GetPostComments(IRepository<Post> repo, IRepository<Comment> commentRepo, IRepository<User>userRepo, int postId, ClaimsPrincipal user)
        {
            Post post = repo.GetById(postId);
            
            PostWithCommentDTO dto = new PostWithCommentDTO();
            dto.postUsername = userRepo.GetById(int.Parse(post.AuthorId)).Username;
            dto.PostId = postId;
            dto.postText = post.Text;

            foreach (Comment comment in commentRepo.GetAll())
            {
                if (comment.PostId == post.Id)
                {
                    CommentWithUser c = new CommentWithUser()
                    {
                        username = userRepo.GetById(comment.UserId).Username,
                        comment = comment.Text
                    };
                    dto.comments.Add(c);
                }
            }

            return TypedResults.Ok(dto);
        }
        

        

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        private static async Task<IResult> Register(UserRequestDTO request, IRepository<User> service)
        {


            if (service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.Conflict(new Payload<UserRequestDTO>() { status = "Username already exists!", data = request });

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User();

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.Email = request.Email;

            service.Insert(user);
            service.Save();

            return Results.Ok(new Payload<string>() { data = "Created Account" });
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        private static async Task<IResult> Login(UserRequestDTO request, IRepository<User> service, IConfigurationSettings config)
        {
            //user doesn't exist
            if (!service.GetAll().Where(u => u.Username == request.Username).Any()) return Results.BadRequest(new Payload<UserRequestDTO>() { status = "User does not exist", data = request });

            User user = service.GetAll().FirstOrDefault(u => u.Username == request.Username)!;


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
