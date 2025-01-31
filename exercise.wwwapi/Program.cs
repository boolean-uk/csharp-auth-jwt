using System.Text;
using exercise.wwwapi.Configuration;
using exercise.wwwapi.Data;
using exercise.wwwapi.Endpoint;
using exercise.wwwapi.Model;
using exercise.wwwapi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddLogging();
var config = new ConfigurationSettings();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnectionString")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IBlogPostRepository, BlogPostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IConfigurationSettings, ConfigurationSettings>();
builder.Services.AddScoped<ILogger, Logger<string>>();

builder.Services.AddCors();

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
        ValidateIssuer = false, // Don't validate issuer
        ValidateAudience = false, // Don't validate audience
        ValidateLifetime = true, // Optionally validate lifetime (expiration) if you have it
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOrAuthor", policy =>
        policy.RequireRole("Admin", "Author"));

    options.AddPolicy("Admin", policy =>
    policy.RequireRole("Admin"));
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
        Description = "Add an Authorization header with a JWT token using the Bearer scheme.",
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
            new string[] { }
        }
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userRepository = services.GetRequiredService<IUserRepository>();
    var context = services.GetRequiredService<BlogDbContext>();
    await DataSeeder.SeedDataAsync(context);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .SetIsOriginAllowed(origin => true) // allow any origin
                  .AllowCredentials()); // allow credentials

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureBlogPostEndpoints();
app.ConfigureUserEndpoints();
app.ConfigureAuthEndpoints();
app.ConfigureCommentEndpoints();

app.Run();