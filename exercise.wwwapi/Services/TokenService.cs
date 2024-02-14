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
        private const int ExpirationMinutes = 60;//how many minutes should the token exist, before the user needs to log in again
        private readonly ILogger<TokenService> _logger;//for printing useful messages
        public TokenService(ILogger<TokenService> logger)
        {
            _logger = logger;
        }
        public string CreateToken(ApplicationUser user) //recieve a user
        {
            var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);// get the expiration as a DateTime, to todays current time add the expiration time
            var token = CreateJwtToken( //calling create Jwt Token function from below
                CreateClaims(user), // the claims for the user
                CreateSigningCredentials(), //the sign in credentlials
                expiration //the expiration
            ); //so the token will recieve all the payload data input for jwt

            var tokenHandler = new JwtSecurityTokenHandler();//generate a jwt Token handler class, which recieves the details for the token and converts them into a
                                                             //string which can be returned to the user

            _logger.LogInformation("JWT Token created");

            return tokenHandler.WriteToken(token); //return a token
        }


        private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, // returns a Jwt secure token which includes  
                                                                                                    
        DateTime expiration) =>
        new( 
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidIssuer"], //the issuer from the appsettings
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["ValidAudience"], //the audience
            claims, //a list of additional properties that we find interesting and important to include in the toke, list of claims found below
            expires: expiration, //the expiry date which was passed as a parameter
            signingCredentials: credentials //all of the sign in credentials found below
        );



        private List<Claim> CreateClaims(ApplicationUser user) //list of all of the interesting properties (can be modified to inlcude whatever is appropriate)
        {
            var jwtSub = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["JwtRegisteredClaimNamesSub"];
            try
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, jwtSub), //the subject
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // the unique identifier
                    new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()), //issued at 
                    new Claim(ClaimTypes.NameIdentifier, user.Id), //name identifier, which stands for the identifier for this user, retrieved from the
                                                                   //actual user that was loaded
                    new Claim(ClaimTypes.Name, user.UserName), //storing the user name
                    new Claim(ClaimTypes.Email, user.Email), //storing the user email
                    new Claim(ClaimTypes.Role, user.Role.ToString()) // want to store a role, so want to specify what the role is, so going to the
                                                                     // enum and storing it as a string
                };
                return claims; //returning the claims
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }



        private SigningCredentials CreateSigningCredentials()
        {
            //the secret key
            var symmetricSecurityKey = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("JwtTokenSettings")["SymmetricSecurityKey"];
            
            //return the sign in credentials with the secret key and algortihm that we are going to use 
            return new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)), SecurityAlgorithms.HmacSha256
            );
        }



    }
}