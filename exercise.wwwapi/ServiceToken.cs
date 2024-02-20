


using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace exercise.wwwapi
{
    public class TokenService
    {
        private const int ExpirationMins = 60;
        private readonly ILogger<TokenService> _logger;
        
        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }

        public string CreateToken(AppUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMins);
            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            _logger.LogInformation("JWT token created");

            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials signingCredentials, DateTime expiration) =>
            new(new ConfigurationBuilder().AddJsonFile("appsettings.example.json").Build().GetSection("JwtTokenSettings") ["ValidIssuer"],
                new ConfigurationBuilder().AddJsonFile("appsettings.example.json").Build().GetSection("JwtTokenSettings") ["ValidAudience"],
                claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );

        private List<Claim> CreateClaims(AppUser user)
        {
            var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.example.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];

            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwtSub),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };

                return claims;
            }

            catch(Exception e) 
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private SigningCredentials CreateSigningCredentials()
        {
            var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];

            return new SigningCredentials(
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(symmetricSecurityKey)
                ),
                SecurityAlgorithms.HmacSha256
            );
        }
    }
}
