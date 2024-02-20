using exercise.wwwapi.DataModels;
using exercise.wwwapi.DTO;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace exercise.wwwapi.Controllers
{
    [Route("/api/account")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signingManager;
        private readonly ITokenService _tokenService;
        public UserController(UserManager<ApplicationUser> userManager, ITokenService tokenService, SignInManager<ApplicationUser> signinManager)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signingManager = signinManager;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegistrationDTO regDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var appUser = new ApplicationUser
                {
                    UserName = regDTO.Username,
                    Email = regDTO.Email,
                };

                var createUser = await _userManager.CreateAsync(appUser, regDTO.Password!);
                if(createUser.Succeeded)
                {
                    return  Ok(new ResponseDTO
                    {
                        Username = appUser.UserName,
                        Email = appUser.Email,
                        Token = _tokenService.CreateToken(appUser)
                    });
                }
                else
                {
                    return StatusCode(500, "Something bad happened");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO logDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.Users.FirstOrDefault(x => x.UserName == logDTO.Username.ToLower());
            if(user == null)
            {
                return Unauthorized("Invalid Username");
            }
            var result = await _signingManager.CheckPasswordSignInAsync(user, logDTO.Password, false);

            if (!result.Succeeded) return Unauthorized("Login Failed");

            return Ok(new ResponseDTO
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
        }

    }
}
