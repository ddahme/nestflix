using Api.Entities;
using Api.Pagination;
using NetTopologySuite.Geometries;

namespace Api.Repositories;

public interface IBoxRepository
{
    Task AddBox(BoxEntity entity);
    Task UpdateBox(BoxEntity entity);
    Task ArchiveBox(BoxEntity entity);
    Task<BoxEntity?> GetBox(Guid id);
    Task<BoxEntity> GetBoxElseThrow(Guid id);
    Task<IEnumerable<BoxEntity>> GetBoxesInDistance(Point point, double radiusInMeter, PageRequestDto page);
}
