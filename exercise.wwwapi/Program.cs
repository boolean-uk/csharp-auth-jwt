//tokens for jwt
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
//own data
using exercise.wwwapi.Data;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repositories;
//authentication
using Microsoft.AspNetCore.Identity;
//databse
using Microsoft.EntityFrameworkCore;
using exercise.wwwapi.Services;
using Microsoft.OpenApi.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
         //option.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // want to let swagger know that we are going to use a particualr security scheme
    {
        In = ParameterLocation.Header, // want this security scheme to be accesible in the header of the web-page
        Description = "Please enter a valid token",
        Name = "Authorization", // it is going to an authorization
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer" //it is going to be bearer type with the jwt process
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            }, 
            new string[] { } 
        }
    });
});
    
  
builder.Services.AddDbContext<DataContext>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<TokenService, TokenService>(); //no interface, could potentially choose to have one, but is okay not to 



// want to have an identity system, that uses the ApplicationUser and identityRole
// the followin is essentially saying that "I want to log in using username and password - in a nutshell
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; //don't care wheter the email is verified or not, not enabling that a user cannot log if they haven't verified their email. 
        options.User.RequireUniqueEmail = true; // require a unique email
        //certain characteristics, the password has to satisfy
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
 // enable authorization using Roles
 .AddRoles<IdentityRole>()
 // specify what database context to use for storing the 
//user data + tables
 .AddEntityFrameworkStores<DataContext>();



// These will eventually be moved to a secrets file, but for alpha development appsettings is fine

// load secrets about our token in the appsettings.json
var validIssuer = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidIssuer"); //typiccaly the name of the server or the ip of the server etc
var validAudience = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidAudience");//who will receive and handle the token , ie. the name of the app or som identifier
//audience can typically be omitted, but good practice to include 

// this is the secret we are going to use to sign / verify the  token
var symmetricSecurityKey = builder.Configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey");


builder.Services.AddAuthorization();// enable the authorization package

//adds and enables the authentication package
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //options for the authentication by jwt
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // add the specifc options for the jwt tokens
   .AddJwtBearer(options =>
   {
       options.IncludeErrorDetails = true; //include all the errors
       options.TokenValidationParameters = new TokenValidationParameters() //what checks to inlude when generating a token
       {
           ClockSkew = TimeSpan.Zero,
           ValidateIssuer = true, //check that the issuer is the same to the one we specify here, in our app
           ValidateAudience = true,
           ValidateLifetime = true,
           ValidateIssuerSigningKey = true,
           ValidIssuer = validIssuer,//providing the issuer to check
           ValidAudience = validAudience,
           IssuerSigningKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(symmetricSecurityKey)
           ),
       };
   });





var app = builder.Build();

// ensure db context is created with seeded data
using (var dbContext = new DataContext(new DbContextOptions<DataContext>()))
{
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigurePostsApi();
app.ConfigureAuthEndpoints();
//app.ApplyProjectMigrations();  might need a migration runner?

app.Run();
