using Api.Clients;
using Api.DTOs;
using Api.Entities;
using Api.Mapper;
using Api.Pagination;
using Api.Repositories;
using Azure.Storage.Sas;

namespace Api.Services;

public sealed class TweetService(ILogger<TweetService> logger,IAiService aiService, IBoxRepository boxRepository, ITweetRepository tweetRepository, IBlobClient blobClient) : ITweetService
{

    public async Task UploadTweet(Guid boxId, CreateTweetRequest request)
    {
        await boxRepository.GetBoxElseThrow(boxId);
        using MemoryStream stream = new(Convert.FromBase64String(request.ImageBase64));
        
        AIResponse? response = null;
        try
        {
            response = await aiService.AnalyzeImageAsync(new AIRequest() { Image = stream });
            stream.Position = 0;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
        }
        
        using MemoryStream stream2 = new(Convert.FromBase64String(request.ImageBase64));
        string fileName = Guid.CreateVersion7().ToString();
        await blobClient.UploadData(fileName, stream2);
        TweetEntity entity = new()
        {
            BirdType = request.BirdType ?? response?.BirdType,
            BoxId = boxId,
            UploadedAt = DateTime.UtcNow,
            BlobName = fileName,
            Description = request.Description,
            EggCount = request.EggCount ?? response?.EggCount,
            HatchedCount = request.HatchedCount ?? response?.HatchedCount,
            IsOccupied = request.IsOccupied ?? response?.IsOccupied,
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