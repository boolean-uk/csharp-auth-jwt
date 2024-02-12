using System.Text;
using exercise.wwwapi.Data;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
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
builder.Services.AddDbContext<DatabaseContext>();
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<TokenService,TokenService>();

builder.Services

.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
// enaable authorization using Roles
.AddRoles<IdentityRole>()
// specify what database context to use for storing the user data + tables
.AddEntityFrameworkStores<DatabaseContext>();

var validIssuer = builder.Configuration.GetValue<string> ("JwtTokenSettings:ValidIssuer");
var validAudience = builder.Configuration.GetValue<string> ("JwtTokenSettings:ValidAudience");
// this is the secret we are going to use to sign / verify the token
var symmetricSecurityKey = builder.Configuration.GetValue<string> ("JwtTokenSettings:SymmetricSecurityKey");

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme =
        JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
        {
        options.IncludeErrorDetails = true;
        options.TokenValidationParameters = new
        TokenValidationParameters()
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = validIssuer,
            ValidAudience = validAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(symmetricSecurityKey)),
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
app.ConfigureAuthEndpoint();
app.ConfigureBlogpostEndpoint();
app.UseAuthentication();
app.UseAuthorization();


app.Run();

public partial class Program { }
