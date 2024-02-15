using exercise.wwwapi.Data;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DataTransfer.Requests;
using exercise.wwwapi.DataTransfer.Response;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly UserManager<Author> _authorManager;
        private readonly DataContext _context;
        private readonly TokenService _tokenService;

        public AuthorsController(UserManager<Author> authorManager, DataContext context,
            TokenService tokenService, ILogger<AuthorsController> logger)
        {
            _authorManager = authorManager;
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authorManager.CreateAsync(
                new Author { UserName = request.Username, Email = request.Email, Role = request.Role },
                request.Password!
            );

            if (result.Succeeded)
            {
                request.Password = "";
                return CreatedAtAction(nameof(Register), new { email = request.Email, role = Role.User }, request);
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }

            return BadRequest(ModelState); 
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<AuthResponse>> Authenticate([FromBody] AuthRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var managedAuthor = await _authorManager.FindByEmailAsync(request.Email!);

            if (managedAuthor == null)
            {
                return BadRequest("Bad Credentials");
            }

            var isPasswordValid = await _authorManager.CheckPasswordAsync(managedAuthor, request.Password!);

            if (!isPasswordValid)
            {
                return BadRequest("Bad Credentials");
            }

            var authorInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

            if(authorInDb is null)
            {
                return Unauthorized();
            }

            var accessToken = _tokenService.CreateToken(authorInDb);
            await _context.SaveChangesAsync();

            return Ok(new AuthResponse
            {
                Username = authorInDb.UserName,
                Email = authorInDb.Email,
                Token = accessToken,
            });


        }

    }
}
