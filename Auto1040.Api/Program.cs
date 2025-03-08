using Auto1040.Api;
using Auto1040.Api.Extensions;
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

builder.Services.AddDependencyInjectoions();

builder.Services.AddSwagger();

builder.AddJwtAuthentication();
builder.AddJwtAuthorization();

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
