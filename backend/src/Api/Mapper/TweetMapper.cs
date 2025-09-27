using Api.DTOs;
using Api.Entities;

namespace Api.Mapper;

public static class TweetMapper
{
    public static TweetResponse ToTweetResponse(this TweetEntity entity, Uri SasUri)
    {
        return new()
        {
            BoxId = entity.BoxId,
            Description = entity.Description,
            IsOccupied = entity.IsOccupied,
            SasUri = SasUri,
            UploadedAt = entity.UploadedAt,
            BirdType = entity.BirdType,
            DeadCount = entity.DeadCount,
            EggCount = entity.EggCount,
            HatchedCount = entity.HatchedCount
        };
    }
}
