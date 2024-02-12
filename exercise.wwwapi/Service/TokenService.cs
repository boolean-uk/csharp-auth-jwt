using exercise.wwwapi.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Service
{
    public class TokenService
    {

        private const int ExpriationMinutes = 60;
        private readonly ILogger<TokenService> _logger;

        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }

        public string CreateToken(ApplicationUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpriationMinutes);
            var token = CreateJwtToken(CreateClaims(user), CreateSigningCredentials(), expiration);
            var TokenHandler = new JwtSecurityTokenHandler();
            _logger.LogInformation("JWT token created.");
            return TokenHandler.WriteToken(token);
        }
        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration) =>
    new JwtSecurityToken(
        issuer: new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
        audience: new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
        claims: claims,
        expires: expiration,
        signingCredentials: credentials
    );

        private List<Claim> CreateClaims(ApplicationUser user)
        {
            var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];

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
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        private SigningCredentials CreateSigningCredentials()
        {
            var symmetricSecurityKey = new
            ConfigurationBuilder().AddJsonFile("appsettings.json").Build()
            .GetSection("JwtTokenSettings")["SymmetricSecurityKey"];


            return new SigningCredentials(
            new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
            SecurityAlgorithms.HmacSha256
            );
        }

    }
}
