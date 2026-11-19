using Microsoft.EntityFrameworkCore;
using Takwene.TrackManagement.Domain.Entities;

namespace Takwene.TrackManagement.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<DSP> DSPs => Set<DSP>();
    public DbSet<TrackDistribution> TrackDistributions => Set<TrackDistribution>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Track configuration
        modelBuilder.Entity<Track>(entity => {
            entity.HasIndex(t => t.Isrc).IsUnique();
            entity.Property(t => t.Isrc).IsRequired().HasMaxLength(12);
            entity.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
            entity.HasOne(t => t.Artist).WithMany(a => a.Tracks).HasForeignKey(t => t.ArtistId).OnDelete(DeleteBehavior.Restrict);
        });
        // TrackDistribution configuration
        modelBuilder.Entity<TrackDistribution>(entity => {
            entity.HasIndex(td => new { td.TrackId, td.DspId }).IsUnique();
            entity.Property(td => td.Status).HasConversion<string>().HasMaxLength(20);
            entity.HasOne(td => td.Track).WithMany(t => t.TrackDistributions).HasForeignKey(td => td.TrackId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(td => td.Dsp).WithMany(d => d.TrackDistributions).HasForeignKey(td => td.DspId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}