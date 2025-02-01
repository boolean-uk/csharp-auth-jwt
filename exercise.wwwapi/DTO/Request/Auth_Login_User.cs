using System.ComponentModel.DataAnnotations.Schema;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api_cinema_challenge.DTO.Interfaces;
using api_cinema_challenge.Repository;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Models;
using Microsoft.IdentityModel.Tokens;

namespace exercise.wwwapi.DTO.Request
{
    public class Auth_Login_User : DTO_Auth_Request<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        protected override async Task<string> CreateAndReturnJWTToken(IRepository<User> repo, User model, IConfigurationSettings conf)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, model.Id.ToString()),
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, model.Role), 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf.GetValue<string>("AppSettings:Token")!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        protected override async Task<User?> ReturnCreatedInstanceModel(IRepository<User> repo)
        {
            return await repo.GetEntry(x => x.Where(x => x.Username == this.Username));
        }

        protected override async Task<bool> VerifyRequestedModelAgainstDTO(IRepository<User> repo, User model)
        {
            if (!BCrypt.Net.BCrypt.Verify(this.Password, model.PasswordHash))
                return false;
            
            return true;
        }
    }
}
