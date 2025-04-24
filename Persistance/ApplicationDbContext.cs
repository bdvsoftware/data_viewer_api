using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DataViewerApi.Persistance.Entity;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<DrivereyeFrame> DrivereyeFrames { get; set; }

    public virtual DbSet<Frame> Frames { get; set; }

    public virtual DbSet<GrandPrix> GrandPrixes { get; set; }

    public virtual DbSet<OnboardFrame> OnboardFrames { get; set; }

    public virtual DbSet<PitboostFrame> PitboostFrames { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<SessionType> SessionTypes { get; set; }

    public virtual DbSet<Team> Teams { get; set; }

    public virtual DbSet<Video> Videos { get; set; }

    public virtual DbSet<WideshotFrame> WideshotFrames { get; set; }
    
    public virtual DbSet<BatteryFrame> BatteryFrames { get; set; }
    
    public virtual DbSet<BatteryFrameDriver> BatteryFrameDrivers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost:5432;Database=data_viewer;Username=postgres;Password=root");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Driver>(entity =>
        {
            entity.HasKey(e => e.DriverId).HasName("driver_pkey");

            entity.ToTable("driver");

            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Abbreviation).HasColumnName("abbreviation");
            entity.Property(e => e.TeamId).HasColumnName("team_id");

            entity.HasOne(d => d.Team).WithMany(p => p.Drivers)
                .HasForeignKey(d => d.TeamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("driver_team_id_fkey");

            entity.HasMany(d => d.WideshotFrames).WithMany(p => p.Drivers)
                .UsingEntity<Dictionary<string, object>>(
                    "WideshotFrameDriver",
                    r => r.HasOne<WideshotFrame>().WithMany()
                        .HasForeignKey("WideshotFrameId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("wideshot_frame_driver_wideshot_frame_id_fkey"),
                    l => l.HasOne<Driver>().WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("wideshot_frame_driver_driver_id_fkey"),
                    j =>
                    {
                        j.HasKey("DriverId", "WideshotFrameId").HasName("wideshot_frame_driver_pkey");
                        j.ToTable("wideshot_frame_driver");
                        j.IndexerProperty<int>("DriverId").HasColumnName("driver_id");
                        j.IndexerProperty<int>("WideshotFrameId").HasColumnName("wideshot_frame_id");
                    });
        });

        modelBuilder.Entity<DrivereyeFrame>(entity =>
        {
            entity.HasKey(e => e.DrivereyeFrameId).HasName("drivereye_frame_pkey");

            entity.ToTable("drivereye_frame");

            entity.Property(e => e.DrivereyeFrameId).HasColumnName("drivereye_frame_id");
            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.FrameId).HasColumnName("frame_id");

            entity.HasOne(d => d.Driver).WithMany(p => p.DrivereyeFrames)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("drivereye_frame_driver_id_fkey");

            entity.HasOne(d => d.Frame).WithMany(p => p.DrivereyeFrames)
                .HasForeignKey(d => d.FrameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("drivereye_frame_frame_id_fkey");
        });

        modelBuilder.Entity<Frame>(entity =>
        {
            entity.HasKey(e => e.FrameId).HasName("frame_pkey");

            entity.ToTable("frame");

            entity.Property(e => e.FrameId).HasColumnName("frame_id");
            entity.Property(e => e.Seq).HasColumnName("seq");
            entity.Property(e => e.Timestamp).HasColumnName("timestamp");
            entity.Property(e => e.VideoId).HasColumnName("video_id");

            entity.HasOne(d => d.Video).WithMany(p => p.Frames)
                .HasForeignKey(d => d.VideoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("frame_video_id_fkey");
        });

        modelBuilder.Entity<GrandPrix>(entity =>
        {
            entity.HasKey(e => e.GpId).HasName("grand_prix_pkey");

            entity.ToTable("grand_prix");

            entity.Property(e => e.GpId).HasColumnName("gp_id");
            entity.Property(e => e.Circuit)
                .HasMaxLength(255)
                .HasColumnName("circuit");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<OnboardFrame>(entity =>
        {
            entity.HasKey(e => e.OnboardFrameId).HasName("onboard_frame_pkey");

            entity.ToTable("onboard_frame");

            entity.Property(e => e.OnboardFrameId).HasColumnName("onboard_frame_id");
            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.FrameId).HasColumnName("frame_id");

            entity.HasOne(d => d.Driver).WithMany(p => p.OnboardFrames)
                .HasForeignKey(d => d.DriverId)
                .HasConstraintName("onboard_frame_driver_id_fkey");

            entity.HasOne(d => d.Frame).WithMany(p => p.OnboardFrames)
                .HasForeignKey(d => d.FrameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("onboard_frame_frame_id_fkey");
        });

        modelBuilder.Entity<PitboostFrame>(entity =>
        {
            entity.HasKey(e => e.PitboostFrameId).HasName("pitboost_frame_pkey");

            entity.ToTable("pitboost_frame");

            entity.Property(e => e.PitboostFrameId).HasColumnName("pitboost_frame_id");
            entity.Property(e => e.FrameId).HasColumnName("frame_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.Frame).WithMany(p => p.PitboostFrames)
                .HasForeignKey(d => d.FrameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pitboost_frame_frame_id_fkey");

            entity.HasMany(d => d.Drivers).WithMany(p => p.PitboostFrames)
                .UsingEntity<Dictionary<string, object>>(
                    "PitboostFrameDriver",
                    r => r.HasOne<Driver>().WithMany()
                        .HasForeignKey("DriverId")
                        .HasConstraintName("pitboost_frame_driver_driver_id_fkey"),
                    l => l.HasOne<PitboostFrame>().WithMany()
                        .HasForeignKey("PitboostFrameId")
                        .HasConstraintName("pitboost_frame_driver_pitboost_frame_id_fkey"),
                    j =>
                    {
                        j.HasKey("PitboostFrameId", "DriverId").HasName("pitboost_frame_driver_pkey");
                        j.ToTable("pitboost_frame_driver");
                        j.IndexerProperty<int>("PitboostFrameId").HasColumnName("pitboost_frame_id");
                        j.IndexerProperty<int>("DriverId").HasColumnName("driver_id");
                    });
        });
        
        modelBuilder.Entity<BatteryFrame>(entity =>
        {
            entity.HasKey(e => e.BatteryFrameId)
                .HasName("battery_frame_pkey");

            entity.ToTable("battery_frame");

            entity.Property(e => e.BatteryFrameId).HasColumnName("battery_frame_id");
            entity.Property(e => e.FrameId).HasColumnName("frame_id");
            entity.Property(e => e.Lap).HasColumnName("lap");

            entity.HasOne(e => e.Frame)
                .WithMany(f => f.BatteryFrames)
                .HasForeignKey(e => e.FrameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pitboost_frame_frame_id_fkey");

            entity.HasMany(e => e.BatteryFrameDrivers)
                .WithOne(d => d.BatteryFrame)
                .HasForeignKey(d => d.BatteryFrameId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("battery_frame_driver_battery_frame_id_fkey");
        });

        
        modelBuilder.Entity<BatteryFrameDriver>(entity =>
        {
            entity.ToTable("battery_frame_driver");

            entity.HasKey(e => new { e.BatteryFrameId, e.DriverId })
                .HasName("battery_frame_driver_pkey");

            entity.Property(e => e.BatteryFrameId).HasColumnName("battery_frame_id");
            entity.Property(e => e.DriverId).HasColumnName("driver_id");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(e => e.BatteryFrame)
                .WithMany(b => b.BatteryFrameDrivers)
                .HasForeignKey(e => e.BatteryFrameId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("battery_frame_driver_battery_frame_id_fkey");

            entity.HasOne(e => e.Driver)
                .WithMany(d => d.BatteryFrameDrivers)
                .HasForeignKey(e => e.DriverId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("battery_frame_driver_driver_id_fkey");
        });


        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasKey(e => e.SessionId).HasName("session_pkey");

            entity.ToTable("session");

            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.Date)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.GpId).HasColumnName("gp_id");
            entity.Property(e => e.SessionTypeId).HasColumnName("session_type_id");

            entity.HasOne(d => d.Gp).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.GpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("session_gp_id_fkey");

            entity.HasOne(d => d.SessionType).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.SessionTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("session_session_type_id_fkey");
        });

        modelBuilder.Entity<SessionType>(entity =>
        {
            entity.HasKey(e => e.SessionTypeId).HasName("session_type_pkey");

            entity.ToTable("session_type");

            entity.Property(e => e.SessionTypeId).HasColumnName("session_type_id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(e => e.TeamId).HasName("team_pkey");

            entity.ToTable("team");

            entity.Property(e => e.TeamId).HasColumnName("team_id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Video>(entity =>
        {
            entity.HasKey(e => e.VideoId).HasName("video_pkey");
            entity.Property(v => v.VideoId).ValueGeneratedOnAdd();
            
            entity.ToTable("video");

            entity.HasIndex(e => e.SessionId, "video_session_id_key").IsUnique();

            entity.Property(e => e.VideoId).HasColumnName("video_id");
            entity.Property(e => e.Duration).HasColumnName("duration");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.SessionId).HasColumnName("session_id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalFrames).HasColumnName("total_frames");
            entity.Property(e => e.Url).HasColumnName("url");

            entity.HasOne(d => d.Session).WithOne(p => p.Video)
                .HasForeignKey<Video>(d => d.SessionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("video_session_id_fkey");
        });

        modelBuilder.Entity<WideshotFrame>(entity =>
        {
            entity.HasKey(e => e.WideshotFrameId).HasName("wideshot_frame_pkey");

            entity.ToTable("wideshot_frame");

            entity.Property(e => e.WideshotFrameId).HasColumnName("wideshot_frame_id");
            entity.Property(e => e.FrameId).HasColumnName("frame_id");

            entity.HasOne(d => d.Frame).WithMany(p => p.WideshotFrames)
                .HasForeignKey(d => d.FrameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("wideshot_frame_frame_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
