using exercise.wwwapi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace exercise.wwwapi.Services
{
    public class TokenService
    {
        //Expiration time for the token in minutes
        private const int ExpirationMinutes = 60;
        //Logger instance for logging
        private readonly ILogger<TokenService> _logger;

        public TokenService(ILogger<TokenService> logger)
        {
            //Constructor for initializing logger
            _logger = logger;
        }

        // Method for creating JWT token for a given user
        public string CreateToken(ApplicationUser user)
        {
            //Calculate token expiration time
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);

            //Create JWT token
            var token = CreateJwtToken(
                CreateClaims(user), // Generate claims for the user
                CreateSigningCredentials(), // Generate signing credentials for token
                expiration // Token expiration time
            );
            var tokenHandler = new JwtSecurityTokenHandler(); // Token handler instance

            _logger.LogInformation("JWT Token created"); // Log token creation

            return tokenHandler.WriteToken(token);  // Return JWT token as a string
        }

        // Method for creating a JWT token
        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials,
           DateTime expiration) =>
           new(
               new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"],
               new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"],
               claims,
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
