using Api.Database;
using Api.Entities;
using Api.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NetTopologySuite.Geometries;

namespace Api.Repositories;

public sealed class BoxRepository(IDbContextFactory<NestflixDbContext> dbContextFactory) : IBoxRepository
{
    public Task AddBox(BoxEntity entity)
    {
        throw new NotImplementedException();
    }

    public Task ArchiveBox(BoxEntity entity)
    {
        throw new NotImplementedException();
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
