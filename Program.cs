using DataViewerApi.Persistance;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register services for the app
builder.Services.AddControllers();

// Register the repositories and services
//builder.Services.AddScoped<IVideoRepository, VideoRepository>();

// Register database context (replace with your actual connection string)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

var app = builder.Build();

app.UseHttpsRedirection();

// Map controllers (important to handle API routes)
app.MapControllers();

app.Run();