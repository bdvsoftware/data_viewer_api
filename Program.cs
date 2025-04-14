using System.Text.Json;
using DataViewerApi.Persistance.Repository;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register database db (replace with your actual connection string)
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

builder.Services.AddScoped<ISessionTypeRepository, SessionTypeRepository>();
builder.Services.AddScoped<ISessionTypeService, SessionTypeService>();

builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddScoped<IGrandPrixRepository, GrandPrixRepository>();
builder.Services.AddScoped<IGrandPrixService, GrandPrixService>();

builder.Services.AddScoped<IFrameRepository, FrameRepository>();
builder.Services.AddScoped<IFrameService, FrameService>();

builder.Services.AddScoped<FrameKafkaProducer>();

var app = builder.Build();

app.UseHttpsRedirection();

// Map controllers (important to handle API routes)
app.MapControllers();

app.Run();