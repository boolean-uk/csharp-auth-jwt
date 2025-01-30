using exercise.wwwapi.Config;
using exercise.wwwapi.Data;
using exercise.wwwapi.Endpoints;
using exercise.wwwapi.Mapper;
using exercise.wwwapi.Models;
using exercise.wwwapi.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IConfig, Config>();
builder.Services.AddScoped<IRepository<User>, Repository<User>>();
builder.Services.AddDbContext<DataContext>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.ConfigureUserEndpoints();

app.Run();