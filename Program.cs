using System.Text.Json;
using DataViewerApi.Kafka.Consumer;
using DataViewerApi.Kafka.Producer;
using DataViewerApi.Persistance;
using DataViewerApi.Persistance.Repository;
using DataViewerApi.Persistance.Entity;
using DataViewerApi.Service;
using Microsoft.AspNetCore.Http.Features;
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

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 10_000_000_000;
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(10);
    options.Limits.MaxRequestBodySize = long.MaxValue;
});

// Register the repositories and services
builder.Services.AddScoped<ITokenConsumptionRepository, TokenConsumptionRepository>();

builder.Services.AddScoped<IFrameRepository, FrameRepository>();
builder.Services.AddScoped<IFrameService, FrameService>();

builder.Services.AddScoped<IVideoRepository, VideoRepository>();
builder.Services.AddScoped<IVideoService, VideoService>();

builder.Services.AddScoped<ISessionTypeRepository, SessionTypeRepository>();
builder.Services.AddScoped<ISessionTypeService, SessionTypeService>();

builder.Services.AddScoped<ISessionRepository, SessionRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();

builder.Services.AddScoped<IGrandPrixRepository, GrandPrixRepository>();
builder.Services.AddScoped<IGrandPrixService, GrandPrixService>();

builder.Services.AddScoped<IBatteryFrameService, BatteryFrameService>();
builder.Services.AddScoped<IBatteryFrameRepository, BatteryFrameRepository>();
builder.Services.AddScoped<IBatteryFrameDriverRepository, BatteryFrameDriverRepository>();

builder.Services.AddScoped<IOnboardHelmetFrameService, OnboardHelmetFrameService>();
builder.Services.AddScoped<IDrivereyeFrameRepository, DrivereyeFrameRepository>();

builder.Services.AddScoped<ITeamRepository, TeamRepository>();

builder.Services.AddScoped<IOnboardFrameRepository, OnboardFrameRepository>();

builder.Services.AddScoped<IDriverRepository, DriverRepository>();

builder.Services.AddScoped<FrameKafkaProducer>();
builder.Services.AddScoped<VideoToProcessKafkaProducer>();
builder.Services.AddHostedService<FrameProcessedKafkaConsumer>();
builder.Services.AddHostedService<StartVideoProcessingKafkaConsumer>();
var app = builder.Build();

app.UseHttpsRedirection();

// Map controllers (important to handle API routes)
app.MapControllers();

app.Run();