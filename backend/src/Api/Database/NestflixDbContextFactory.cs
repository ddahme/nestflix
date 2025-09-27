using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Api.Database;

// Design-time factory so `dotnet ef` can create the DbContext without needing the full host (and env vars like AZURE_BLOB_STORAGE_CONNECTION).
public sealed class NestflixDbContextFactory : IDesignTimeDbContextFactory<NestflixDbContext>
{
    public NestflixDbContext CreateDbContext(string[] args)
    {
        // Build configuration (appsettings + env vars)
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        // Fallback local connection string if none provided
        var connectionString = configuration.GetConnectionString("Postgres")
                               ?? "Host=localhost;Port=5432;Database=nestflix;Username=postgres;Password=postgres";

        var optionsBuilder = new DbContextOptionsBuilder<NestflixDbContext>();
        optionsBuilder.UseNpgsql(connectionString, npgsql =>
        {
            npgsql.UseNetTopologySuite();
        });

        return new NestflixDbContext(optionsBuilder.Options);
    }
}
