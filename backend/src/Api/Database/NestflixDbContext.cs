using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class NestflixDbContext(DbContextOptions<NestflixDbContext> options) : DbContext(options)
{
    public DbSet<TweetEntity> Tweets { get; set; } = null!;
    public DbSet<BoxEntity> Boxes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");

        modelBuilder.Entity<TweetEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Box)
                  .WithMany(b => b.Tweets)
                  .HasForeignKey(e => e.BoxId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BoxEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            // Store point as geography so distance calculations are in meters
            entity.Property(e => e.Point).HasColumnType("geography (Point,4326)");
            // Spatial index for performant distance queries
            entity.HasIndex(e => e.Point).HasMethod("GIST");
        });
    }
}
