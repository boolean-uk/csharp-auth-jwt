using csharp_auth_jwt.Data;
using csharp_auth_jwt.Endpoints;
using csharp_auth_jwt.Model;
using csharp_auth_jwt.Repository;
using csharp_auth_jwt.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer" , new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header ,
        Description = "Write here tocen" ,
        Name = "Authorization" ,
        Type = SecuritySchemeType.Http ,
        BearerFormat = "JWT" ,
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
builder.Services.AddDbContext<BlogPostContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ElephantSqlConnectionString")));

builder.Services.AddScoped<IBlogPostRepository , BlogPostRepository>();
builder.Services.AddScoped<TokenService>();

builder.Services
    .AddIdentityCore<BlogUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.User.RequireUniqueEmail = true;

        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddRoles<IdentityRole>()
    .AddUserStore<UserStore<BlogUser , IdentityRole , BlogPostContext>>()
    .AddRoleStore<RoleStore<IdentityRole , BlogPostContext>>()
    .AddDefaultTokenProviders();


var validIssuer = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidIssuer");
var validAudience = builder.Configuration.GetValue<string>("JwtTokenSettings:ValidAudience");
var symmetricSecurityKey = builder.Configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey");

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero ,
            ValidateIssuer = true ,
            ValidateAudience = true ,
            ValidateLifetime = true ,
            ValidateIssuerSigningKey = true ,
            ValidIssuer = validIssuer ,
            ValidAudience = validAudience ,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(symmetricSecurityKey)
            ) ,
        };
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureBlogPostEndpoints();
app.ConfigureAuthEndpoints();

app.Run();
