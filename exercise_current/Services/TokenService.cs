using exercise.wwwapi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Services
{
    public class TokenService
    {
        private const int ExpirationMinutes = 60;
        private readonly ILogger<TokenService> _logger;
        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }
        public string CreateToken(ApplicationUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
            var token = CreateJwtToken(
                CreateClaims(user),
                CreateSigningCredentials(),
                expiration
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            _logger.LogInformation("JWT Token created");
            return tokenHandler.WriteToken(token);
        }
        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
            new(
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
                new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
                claims,
                expires: expiration,
                signingCredentials: credentials
        );
        private List<Claim> CreateClaims(ApplicationUser user)
        {//What payload data contains (purple part of token)
            var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                };
                return claims;
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
        private SigningCredentials CreateSigningCredentials()
        {
            //Use systems secret when creating tokens
            var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];
            //Choose algorithm and secret (Red and blue part of JWT)
            return new SigningCredentials( new SymmetricSecurityKey( Encoding.UTF8.GetBytes(symmetricSecurityKey)), SecurityAlgorithms.HmacSha256);
        }
    }
}
