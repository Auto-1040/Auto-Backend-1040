using Amazon;
using Amazon.S3;
using Auto1040.Api;
using Auto1040.Api.Extensions;
using Auto1040.Core;
using Auto1040.Core.Repositories;
using Auto1040.Core.Services;
using Auto1040.Core.Shared;
using Auto1040.Data;
using Auto1040.Data.Repositories;
using Auto1040.Service;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
}); 

builder.Services.AddDbContext<DataContext>();

builder.Services.AddDependencyInjectoions();

builder.Services.AddSwagger();
builder.Services.AddAllowAnyCors();
builder.AddJwtAuthentication();
builder.AddJwtAuthorization();

// Configure AWS settings

var awsSettings = new AwsSettings
{
    AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID"),
    SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY"),
    Region = Environment.GetEnvironmentVariable("AWS_REGION")
};

var s3Client = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, RegionEndpoint.GetBySystemName(awsSettings.Region));
builder.Services.AddSingleton<IAmazonS3>(s3Client);


var app = builder.Build();

if (app.Environment.IsDevelopment()|| app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(CorsExtension.MyAllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
