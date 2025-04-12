using System.Text.Json;
using DataViewerApi.Persistance.Repository;
using DataViewerApi.Prueba;
using DataViewerApi.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register database context (replace with your actual connection string)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

// Register services for the app
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

// Register the repositories and services
builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IVideoService, VideoService>();


var app = builder.Build();

app.UseHttpsRedirection();

// Map controllers (important to handle API routes)
app.MapControllers();

app.Run();