// JWT TOKENS
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
//database
using Microsoft.EntityFrameworkCore;
//authentication
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
//own models
using exercise.wwwapi.Data;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Models;
using exercise.wwwapi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//tell swagger we want to use a security scheme accesible in the header of the webpage at /swagger
//this to enable the a place to paste the jwt token so we can test authorized endpoints in swagger ui
builder.Services.AddSwaggerGen(option =>
{
    //option.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
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


builder.Services.AddDbContext<DatabaseContext>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<TokenService, TokenService>(); //same exact class, no interface in this case. Could use interface

//define wanting an identity system
//Options to login with username and password
builder.Services
    .AddIdentity<Users, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // dont need to verify email
        options.User.RequireUniqueEmail = true;

        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>() //app knows we can have roles, role based auth, admin or user
    .AddEntityFrameworkStores<DatabaseContext>(); //this is the database we want to use for seeding and representing all the user tables

// These will eventually be moved to a secrets file, but for alpha development appsettings is fine
//Load secrets about our tokens in the appsettings.json
var validIssuer = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidIssuer");
var validAudience = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidAudience");
//this is the secret we are going to use sign and verify the token
var symmetricSecurityKey = builder.Configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey");

builder.Services.AddAuthorization(); // enable the autherization package in the app

//options for the authentication
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // set schemes for auth
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        //add specific options for jwt token
        options.IncludeErrorDetails = true;
        //what checks we want to make when we recieve a token or generate one
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ),
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // use jwt auth
app.UseAuthorization(); // use jwt authorization

app.ConfigureApi();
app.ConfigureAuthApi();
app.ApplyProjectMigrations();
app.Run();


public partial class Program { } // needed for testing - please ignore