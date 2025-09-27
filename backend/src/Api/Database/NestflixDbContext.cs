using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class NestflixDbContext : DbContext
{
    public DbSet<TweetEntity> Tweets { get; set; } = null!;
    public DbSet<BoxEntity> Boxes { get; set; } = null!;

    public IQueryable<BoxEntity> ActiveBoxes => Boxes.Where(t => !t.IsArchived);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TweetEntity>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.HasOne(e => e.Box)
                  .WithMany()
                  .HasForeignKey(e => e.BoxId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BoxEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}
