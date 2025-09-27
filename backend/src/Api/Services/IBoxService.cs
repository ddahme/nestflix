using Api.DTOs;

namespace Api.Services;

public interface IBoxService
{
    Task CreateBox(CreateBoxRequest request);
    Task UpdateBox(Guid boxId, UpdateBoxRequest request);
    Task DeleteBox(Guid boxId);
    Task<BoxResponse> GetBoxById(Guid id);
    Task<IEnumerable<BoxResponse>> GetBoxesInDistance(double longitude, double latitude, int radiusInMeter);
}
