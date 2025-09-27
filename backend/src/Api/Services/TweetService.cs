using Api.Clients;
using Api.DTOs;
using Api.Entities;
using Api.Mapper;
using Api.Pagination;
using Api.Repositories;
using Azure.Storage.Sas;

namespace Api.Services;

public sealed class TweetService(IBoxRepository boxRepository, ITweetRepository tweetRepository, IBlobClient blobClient) : ITweetService
{

    public async Task UploadTweet(Guid boxId, CreateTweetRequest request)
    {
        await boxRepository.GetBoxElseThrow(boxId);
        using MemoryStream stream = new(Convert.FromBase64String(request.ImageBase64));
        string fileName = Guid.CreateVersion7().ToString();
        await blobClient.UploadData(fileName, stream);
        TweetEntity entity = new()
        {
            BirdType = request.BirdType,
            BoxId = boxId,
            UploadedAt = DateTime.UtcNow,
            BlobName = fileName,
            Description = request.Description,
            EggCount = request.EggCount,
            HatchedCount = request.HatchedCount,
            IsOccupied = request.IsOccupied,
            DeadCount = request.DeadCount
        };
        await tweetRepository.AddTweet(entity);
    }

    public async Task<IEnumerable<TweetResponse>> GetTweetsFromBox(Guid boxId, PageRequestDto page)
    {
        await boxRepository.GetBoxElseThrow(boxId);
        List<TweetEntity> tweets = await tweetRepository.GetTweetsFromBox(boxId, page);
        return tweets.Select(t =>
        {
            Uri sasUri = blobClient.GenerateSaSUri(t.BlobName, BlobContainerSasPermissions.Read);
            return t.ToTweetResponse(sasUri);
        });
    }
}