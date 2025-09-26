using Api.DTOs;
using Api.Entities;
using Api.Mapper;
using Api.Repositories;

namespace Api.Services;

public sealed class BoxService(IBoxRepository repository, ITweetRepository tweetRepository) : IBoxService
{

    public async Task CreateBox(CreateBoxRequest request)
    {
        BoxEntity entity = new()
        {
            Name = request.Name,
            BoxType = request.BoxType,
            CreatedAt = DateTime.UtcNow,
            Point = request.Point.ToPoint()
        };
        await repository.AddBox(entity);
    }

    public async Task ArchiveBox(Guid id)
    {
        BoxEntity entity = await repository.GetBoxElseThrow(id);
        await repository.ArchiveBox(entity);
    }

    public async Task<BoxResponse> GetBoxById(Guid id)
    {
        BoxEntity entity = await repository.GetBoxElseThrow(id);
        TweetEntity? latestTweet = await tweetRepository.GetLatestTweet(id);
        return entity.ToBoxResponse(latestTweet?.);
    }

    public Task<IEnumerable<BoxResponse>> GetBoxesInDistance(double longitude, double latitude, int radiusInMeter)
    {
        throw new NotImplementedException();
    }

    public Task UpdateBox(Guid id, UpdateBoxRequest request)
    {
        throw new NotImplementedException();
    }
}
