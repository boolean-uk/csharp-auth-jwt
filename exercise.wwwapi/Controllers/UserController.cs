using exercise.wwwapi.Data;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.DTOs.User;
using exercise.wwwapi.Enums;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
   
        //KANTHEE: This is an oldway: using controller!! 
        //But this is a way to setup User ccontroller "endpoint", register and login.

        //[ApiVersion( "1.0" )]
        [ApiController]
        [Route("/api/[controller]")]
        public class UsersController : ControllerBase
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly DataContext _context;
            private readonly TokenService _tokenService;

            public UsersController(UserManager<ApplicationUser> userManager, DataContext context,
                TokenService tokenService, ILogger<UsersController> logger)
            {
                _userManager = userManager;
                _context = context;
                _tokenService = tokenService;
            }


            [HttpPost]
            [Route("register")]
            public async Task<IActionResult> Register(InRegisterDTO request)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userManager.CreateAsync(
                    new ApplicationUser { UserName = request.Username, Email = request.Email, Role = request.Role },
                    request.Password!       //SICK USerManger Class check if the input password is valid etc....
                );

                if (result.Succeeded)
                {
                    request.Password = "";
                    return CreatedAtAction(nameof(Register), new { email = request.Email, role = Role.User }, request);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }


            [HttpPost]
            [Route("login")]
            public async Task<ActionResult<OutAuthDTO>> Authenticate([FromBody] InAuthDTO request)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                /*   var user = await _userManager.FindAsync(model.UserName, model.Password);

                   if (user == null)
                   {
                       return BadRequest("Invalid username or password.");
                   }*/

                var managedUser = await _userManager.FindByEmailAsync(request.Email!);

                if (managedUser == null)
                {
                    return BadRequest("Bad credentials");
                }

                var isPasswordValid = await _userManager.CheckPasswordAsync(managedUser, request.Password!);

                if (!isPasswordValid)
                {
                    return BadRequest("Bad credentials");
                }

                var userInDb = _context.Users.FirstOrDefault(u => u.Email == request.Email);

                if (userInDb is null)
                {
                    return Unauthorized();
                }

                //KANTHEE: IF THE credentials is fine! Return the token -> this token can be used to access things..
                var accessToken = _tokenService.CreateToken(userInDb);
                await _context.SaveChangesAsync();

                return Ok(new OutAuthDTO
                {
                    Username = userInDb.UserName,
                    Email = userInDb.Email,
                    Token = accessToken,
                });
            }
        }
   
}
    