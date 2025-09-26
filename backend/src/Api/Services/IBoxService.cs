using Api.DTOs;

namespace Api.Services;

public interface IBoxService
{
    Task CreateBox(CreateBoxRequest request);
    Task UpdateBox(Guid id, UpdateBoxRequest request);
    Task ArchiveBox(Guid id);
    Task<BoxResponse> GetBoxById(Guid id);
    Task<IEnumerable<BoxResponse>> GetBoxesInDistance(double longitude, double latitude, int radiusInMeter);
}
