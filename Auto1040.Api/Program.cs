using Auto1040.Api;
using Auto1040.Core;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using Auto1040.Data;
using Auto1040.Data.Repositories;
using Auto1040.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))
    )
);

builder.Services.AddAutoMapper(typeof(MappingProfile),typeof(MappingPostProfile));

// Register repositories
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();

// Add Swagger services
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
