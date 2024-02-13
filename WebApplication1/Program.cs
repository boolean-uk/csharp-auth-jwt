using WebApplication1.Data;
using WebApplication1.Repository;
using WebApplication1.Models;
using WebApplication1.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Text;
using WebApplication1.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BlogContext>();
builder.Services.AddScoped<Irepository, Repository>();
builder.Services.AddScoped<TokenService, TokenService>();

builder.Services
// specify we want email + password login, using our
.AddIdentity<AuthUser, IdentityRole>(options =>
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
// specify what database context to use for storing the
//user data + tables
.AddEntityFrameworkStores<BlogContext>();

var validIssuer = builder.Configuration.GetValue<string>
("JwtTokenSettings:ValidIssuer");

var validAudience = builder.Configuration.GetValue<string>
("JwtTokenSettings:ValidAudience");
// this is the secret we are going to use to sign / verify the
//token
var symmetricSecurityKey =
builder.Configuration.GetValue<string>
("JwtTokenSettings:SymmetricSecurityKey");

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
        IssuerSigningKey = new SymmetricSecurityKey(
    Encoding.UTF8.GetBytes(symmetricSecurityKey)
    ),
    };
});

builder.Services.AddSwaggerGen(option =>
{
//option.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoAPI", Version = "v1" });
option.AddSecurityDefinition("Bearer", new
OpenApiSecurityScheme
{
    In = ParameterLocation.Header,
    Description = "Please enter a valid token",
    Name = "Authorization",
    Type = SecuritySchemeType.Http,
    BearerFormat = "JWT",
    Scheme = "Bearer"
});
    option.AddSecurityRequirement(new
    OpenApiSecurityRequirement
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




var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureBlogPostEndpoint();

//app.UseAuthorization();
app.ConfigureAuthEndpoints();
//app.MapControllers();

app.Run();
