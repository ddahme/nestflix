using Api.Database;
using Api.Entities;
using Api.Exceptions;
using Api.Pagination;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Api.Repositories;

public sealed class BoxRepository(ILogger<BoxRepository> logger, IDbContextFactory<NestflixDbContext> dbContextFactory) : IBoxRepository
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

    public async Task<BoxEntity?> GetBox(Guid id)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        return await context.Boxes.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<BoxEntity> GetBoxElseThrow(Guid id)
    {
        return await GetBox(id) ?? throw new NotFoundException();
    }

    public async Task<IEnumerable<BoxEntity>> GetBoxesInDistance(Point point, double radiusInMeter, PageRequestDto page)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();

        return await context.Boxes
            .Where(b => !b.IsArchived)
            .Where(b => b.Point.Distance(point) <= radiusInMeter)
            .OrderBy(b => b.Point.Distance(point))
            .Skip(page.Offest())
            .Take(page.Size)
            .ToListAsync();
    }

    public async Task UpdateBox(BoxEntity entity)
    {
        await using var context = await dbContextFactory.CreateDbContextAsync();
        context.Boxes.Update(entity);
        await context.SaveChangesAsync();
    }
}
