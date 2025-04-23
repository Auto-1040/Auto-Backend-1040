using Amazon;
using Amazon.S3;
using Auto1040.Api.Extensions;
using Auto1040.Core.Shared;
using Auto1040.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

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
    AccessKey = builder.Configuration["AWS_ACCESS_KEY_ID"],
    SecretKey = builder.Configuration["AWS_SECRET_ACCESS_KEY"],
    Region = builder.Configuration["AWS_REGION"]
};

var s3Client = new AmazonS3Client(awsSettings.AccessKey, awsSettings.SecretKey, RegionEndpoint.GetBySystemName(awsSettings.Region));
builder.Services.AddSingleton<IAmazonS3>(s3Client);

builder.Services.Configure<PythonServiceSettings>(builder.Configuration.GetSection("PythonServiceSettings"));
builder.Services.AddApplicationInsightsTelemetry();

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
