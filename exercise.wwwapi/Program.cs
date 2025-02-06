using exercise.wwwapi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using exercise.wwwapi.Helpers;
using exercise.wwwapi.Repository;
using exercise.wwwapi.Model;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Endpoints; // Add this line

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var config = new ConfigurationSettings();
// Add services to the container.
builder.Services.AddScoped<IConfigurationSettings, ConfigurationSettings>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddScoped<IRepository<Blogpost>, Repository<Blogpost>>();

builder.Services.AddScoped<ILogger, Logger<string>>();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString"));
    options.LogTo(message => Debug.WriteLine(message));
});
//authentication verifying who they say they are
//authorization verifying what they have access to
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetValue("AppSettings:Token"))),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = false
    };
});
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "C# API Authentication",
        Description = "Demo of an API using JWT as an authentication method",
        Contact = new OpenApiContact
        {
            Name = "Nigel",
            Email = "nigel@nigel.nigel",
            Url = new Uri("https://www.boolean.co.uk")
        },
        License = new OpenApiLicense
        {
            Name = "Boolean",
            Url = new Uri("https://github.com/boolean-uk/csharp-api-auth")
        }
    });

    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Add an Authorization header with a JWT token using the Bearer scheme see the app.http file for an example.)",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Demo API");
    });
    // app.MapScalarApiReference(); // Remove this line
}

app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) // allow any origin
                  .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

app.ConfigureBlogApi();
app.ConfigureAuthApi();

app.UseAuthentication();
app.UseAuthorization();

app.Run();