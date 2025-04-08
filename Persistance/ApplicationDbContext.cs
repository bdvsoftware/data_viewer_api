using DataViewerApi.Persistance.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<GrandPrix> GrandPrix { get; set; }
    public DbSet<SessionType> SessionTypes { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<Frame> Frames { get; set; }
    public DbSet<OnboardFrame> OnboardFrames { get; set; }
    public DbSet<DrivereyeFrame> DrivereyeFrames { get; set; }
    public DbSet<PitboostFrame> PitboostFrames { get; set; }
    public DbSet<PitboostFrameDriver> PitboostFrameDrivers { get; set; }
    public DbSet<WideshotFrame> WideshotFrames { get; set; }
    public DbSet<WideshotFrameDriver> WideshotFrameDrivers { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Driver> Drivers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Driver>()
            .ToTable("driver");
        
        modelBuilder.Entity<DrivereyeFrame>()
            .ToTable("drivereye_frame");
        
        modelBuilder.Entity<Frame>()
            .ToTable("frame");
        
        modelBuilder.Entity<GrandPrix>()
            .ToTable("grand_prix");
        
        modelBuilder.Entity<OnboardFrame>()
            .ToTable("onboard_frame");
        
        modelBuilder.Entity<PitboostFrame>()
            .ToTable("pitboost_frame");
        
        modelBuilder.Entity<PitboostFrameDriver>()
            .ToTable("pitboost_frame_driver");

        modelBuilder.Entity<Session>()
            .ToTable("session"); 
        
        modelBuilder.Entity<SessionType>()
            .ToTable("session_type"); 
        
        modelBuilder.Entity<Team>()
            .ToTable("team"); 
        
        modelBuilder.Entity<Video>()
            .ToTable("video"); 
        
        modelBuilder.Entity<WideshotFrame>()
            .ToTable("wideshot_frame"); 
        
        modelBuilder.Entity<WideshotFrameDriver>()
            .ToTable("wideshot_frame_driver"); 
        
        modelBuilder.Entity<WideshotFrameDriver>()
            .HasKey(d => new { d.DriverId, d.WideshotFrameId });
        
        modelBuilder.Entity<PitboostFrameDriver>()
            .HasKey(d => new { d.PitboostFrameId, d.DriverId });
    }
}