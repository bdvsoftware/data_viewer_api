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
    public DbSet<DriverWideshotFrame> DriverWideshotFrames { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Driver> Drivers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.GrandPrix)
            .WithMany(gp => gp.Sessions)
            .HasForeignKey(s => s.GpId);

        modelBuilder.Entity<Session>()
            .HasOne(s => s.SessionType)
            .WithMany(st => st.Sessions)
            .HasForeignKey(s => s.SessionTypeId);

        modelBuilder.Entity<Video>()
            .HasOne(v => v.Session)
            .WithOne(s => s.Video)
            .HasForeignKey<Video>(v => v.SessionId);

        modelBuilder.Entity<Frame>()
            .HasOne(f => f.Video)
            .WithMany(v => v.Frames)
            .HasForeignKey(f => f.VideoId);

        modelBuilder.Entity<OnboardFrame>()
            .HasOne(of => of.Frame)
            .WithMany(f => f.OnboardFrames)
            .HasForeignKey(of => of.FrameId);

        modelBuilder.Entity<DrivereyeFrame>()
            .HasKey(df => new { df.DriverId, df.FrameId });

        modelBuilder.Entity<DrivereyeFrame>()
            .HasOne(df => df.Driver)
            .WithMany() 
            .HasForeignKey(df => df.DriverId);

        modelBuilder.Entity<DrivereyeFrame>()
            .HasOne(df => df.Frame)
            .WithMany() 
            .HasForeignKey(df => df.FrameId);

        modelBuilder.Entity<PitboostFrame>()
            .HasOne(pb => pb.Frame)
            .WithMany(f => f.PitboostFrames)
            .HasForeignKey(pb => pb.FrameId);

        modelBuilder.Entity<WideshotFrame>()
            .HasOne(ws => ws.Frame)
            .WithMany(f => f.WideshotFrames)
            .HasForeignKey(ws => ws.FrameId);

        modelBuilder.Entity<DriverWideshotFrame>()
            .HasKey(d => new { d.DriverId, d.WideshotFrameId });
        
        modelBuilder.Entity<PitboostFrameDriver>()
            .HasKey(d => new { d.PitboostFrameId, d.DriverId });

        modelBuilder.Entity<Driver>()
            .HasOne(d => d.Team)
            .WithMany(t => t.Drivers)
            .HasForeignKey(d => d.TeamId);
    }
}