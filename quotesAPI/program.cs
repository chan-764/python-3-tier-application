using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;  // ✅ Fix: Add missing namespace
using QuotesApi.Data;

var builder = WebApplication.CreateBuilder(args);

// ✅ Extract connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ✅ Ensure MySQL is properly configured
builder.Services.AddDbContext<QuotesDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ✅ Fix: Properly configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Quotes API",
        Version = "v1",
        Description = "API for managing quotes"
    });
});

// ✅ Enable CORS (Allow Flask frontend to call this API)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

// ✅ Ensure API listens on port 5001
app.Urls.Add("http://0.0.0.0:5001");

// ✅ Middleware configuration
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// ✅ Enable Swagger UI for API documentation
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quotes API v1"));
}

app.Run();
