using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using exercise.wwwapi.Config;
using exercise.wwwapi.Models;
using Microsoft.IdentityModel.Tokens;

namespace exercise.wwwapi.Helpers;

public static class Jwt
{
    static IConfig _config;
    
    const string ClaimTypeUsername = "Username";
    
    static Jwt()
    {
        _config = new Config.Config();
    }
    
    public static string CreateToken(User user)
    {
        List<Claim> claims =
        [
            new(ClaimTypes.Sid, user.Id.ToString()),
            new(ClaimTypes.Name, user.DisplayName),
            new(ClaimTypeUsername, user.Username),
            new(ClaimTypes.Email, user.Email)
        ];
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue("JwtTokenSettings:SymmetricSecurityKey")!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _config.GetValue("JwtTokenSettings:Issuer"),
            audience: _config.GetValue("JwtTokenSettings:Audience"),
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public static int? Id(this ClaimsPrincipal claims)
    {
        return int.Parse(claims.FindFirst(ClaimTypes.Sid)!.Value);
    }
    
    public static string DisplayName(this ClaimsPrincipal claims)
    {
        return claims.FindFirst(ClaimTypes.Name)!.Value;
    }
    
    public static string Username(this ClaimsPrincipal claims)
    {
        return claims.FindFirst(ClaimTypeUsername)!.Value;
    }
    
    public static string Email(this ClaimsPrincipal claims)
    {
        return claims.FindFirst(ClaimTypes.Email)!.Value;
    }
}