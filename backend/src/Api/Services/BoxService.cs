using Api.Clients;
using Api.DTOs;
using Api.Entities;
using Api.Mapper;
using Api.Repositories;
using Azure.Storage.Sas;

namespace Api.Services;

public sealed class BoxService(IBoxRepository repository, ITweetRepository tweetRepository, IBlobClient blobClient) : IBoxService
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
        TweetResponse? latestTweetResponse = null;
        if (latestTweet is not null)
        {
            Uri sasUri = blobClient.GenerateSaSUri(latestTweet.BlobName, BlobContainerSasPermissions.Read);
            latestTweetResponse = latestTweet.ToTweetResponse(sasUri);
        }
        return entity.ToBoxResponse(latestTweetResponse);
    }

    public async Task<IEnumerable<BoxResponse>> GetBoxesInDistance(GetBoxesInDistanceRequest request)
    {
        IEnumerable<BoxEntity> boxes = await repository.GetBoxesInDistance(request.Point.ToPoint(), request.DistanceInMeters, request.Page);
        var boxResponseTasks = boxes.Select(async b =>
        {
            TweetEntity? latestTweet = await tweetRepository.GetLatestTweet(b.Id);
            TweetResponse? latestTweetResponse = null;
            if (latestTweet is not null)
            {
                Uri sasUri = blobClient.GenerateSaSUri(latestTweet.BlobName, BlobContainerSasPermissions.Read);
                latestTweetResponse = latestTweet.ToTweetResponse(sasUri);
            }
            return b.ToBoxResponse(latestTweetResponse);
        });
        BoxResponse[] boxResponses = await Task.WhenAll(boxResponseTasks);
        return boxResponses;
    }

    public async Task UpdateBox(Guid id, UpdateBoxRequest request)
    {
        BoxEntity boxEntity = await repository.GetBoxElseThrow(id);
        boxEntity.Name = request.Name;
        await repository.UpdateBox(boxEntity);
    }
}
