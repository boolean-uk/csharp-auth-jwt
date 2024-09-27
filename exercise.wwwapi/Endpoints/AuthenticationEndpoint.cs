using exercise.wwwapi.Data;
using exercise.wwwapi.DataModels;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Identity;

namespace exercise.wwwapi.Endpoints
{
    public static class AuthenticationEndpoint
    {
       

        public static void AuthenticationEndpointConfiguration(this WebApplication app)
        {

            var register = app.MapGroup("register");
            //register.MapPost("/register", Register);
            //register.MapPost("/login", Login);
            
        }
    }
}
