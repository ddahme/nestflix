using Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Database;

public class NestflixDbContext : DbContext
{
    public DbSet<TweetEntity> Tweets { get; set; } = null!;
    public DbSet<BoxEntity> Boxes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }
}
