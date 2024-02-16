using exercise.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace exercise.Application
{
    public class TokenService
    {
        private const int MINUTES_UNTIL_EXPIRY = 10;

        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string CreateToken(User user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(MINUTES_UNTIL_EXPIRY);
            var token = CreateJwtToken(
                claims: CreateClaims(user),
                expiration: expiration,
                credentials: CreateSigningCredentials()
            );
            JwtSecurityTokenHandler tokenHandler = new();

            return tokenHandler.WriteToken(token);
        }

        private JwtSecurityToken CreateJwtToken(List<Claim> claims, 
            SigningCredentials credentials,
            DateTime expiration)
        {
            string validIssuer = _configuration.GetSection("JwtTokenSettings")["ValidIssuer"]
                ?? throw new NullReferenceException("No assigned valid issuer!");
            string validAudience = _configuration.GetSection("JwtTokenSettings")["ValidAudience"]
                ?? throw new NullReferenceException("No assigned valid audience!");

            JwtSecurityToken token = new
            (
                issuer: validIssuer,
                audience: validAudience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return token;
        }

        private List<Claim> CreateClaims(User user)
        {
            var jwtSub = _configuration.GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];

            try
            {
                var claims = new List<Claim>()
                {
                    new(JwtRegisteredClaimNames.Sub, jwtSub),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
                    new(ClaimTypes.Name, user.Id!),
                    new(ClaimTypes.Email, user.Email!),
                    new(ClaimTypes.Role, user.Role.ToString()),
                };

                return claims;
            } catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Could not create claims!");
            }
        }

        private SigningCredentials CreateSigningCredentials()
        {
            string symmetricSecurityKey = _configuration
                .GetSection("JwtTokenSettings")["SymmetricSecurityKey"]
                ?? throw new NullReferenceException("No symmetric security key found!");
            
            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(symmetricSecurityKey));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            return credentials;
        }
    }
}
