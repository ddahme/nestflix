using Api.Pagination;

namespace Api.DTOs;

public sealed record BoxResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
    public required PointDto Point { get; init; }
    public required BoxType BoxType { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required bool IsArchived { get; init; }
    public TweetResponse? LatestTweet { get; set; }
}

public sealed record UpdateBoxRequest
{
    public required string Name { get; init; }
}

public sealed record GetBoxesInDistanceRequest
{
    public required PointDto Point { get; set; }
    public required int DistanceInMeters { get; set; }
    public PageRequestDto Page { get; set; } = new();
}

public sealed record CreateBoxRequest
{
    public required string Name { get; init; }
    public required PointDto Point { get; init; }
    public required BoxType BoxType { get; init; }
}
