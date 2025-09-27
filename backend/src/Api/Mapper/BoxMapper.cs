using Api.DTOs;
using Api.Entities;

namespace Api.Mapper;

public static class BoxMapper
{
    public static BoxResponse ToBoxResponse(this BoxEntity entity, TweetResponse? latestTweet = null)
    {
        return new()
        {
            Id = entity.Id,
            Name = entity.Name,
            BoxType = entity.BoxType,
            CreatedAt = entity.CreatedAt,
            Point = entity.Point.ToPointDto(),
            IsArchived = entity.IsArchived,
            LatestTweet = latestTweet
        };
    }
}
