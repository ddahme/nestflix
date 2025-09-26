using Api.Database;
using Api.Entities;
using Api.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NetTopologySuite.Geometries;

namespace Api.Repositories;

public sealed class BoxRepository(IDbContextFactory<NestflixDbContext> dbContextFactory) : IBoxRepository
{
    public async Task AddBox(BoxEntity entity)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        await context.Boxes.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task ArchiveBox(BoxEntity entity)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        entity.IsArchived = true;
        context.Boxes.Update(entity);
        await context.SaveChangesAsync();
    }

    public Task<BoxEntity?> GetBox(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BoxEntity>> GetBoxesInDistance(Point point, int radiusInMeter, PageRequestDto page)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBox(BoxEntity entity)
    {
        throw new NotImplementedException();
    }
}
