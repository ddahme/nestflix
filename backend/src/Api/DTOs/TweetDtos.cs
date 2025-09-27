namespace Api.DTOs;

public sealed record TweetResponse
{

    public required Guid BoxId { get; init; }
    public required Uri SasUri { get; init; }
    public required DateTime UploadedAt { get; init; }
    public bool? IsOccupied { get; init; }
    public string? BirdType { get; init; }
    public int? EggCount { get; init; }
    public int? HatchedCount { get; init; }
    public int? DeadCount { get; init; }
    public required string Description { get; init; }

}

public sealed record CreateTweetRequest
{
    public required string ImageBase64 { get; init; }

    public bool? IsOccupied { get; init; }
    public string? BirdType { get; init; }
    public int? EggCount { get; init; }
    public int? HatchedCount { get; init; }
    public int? DeadCount { get; init; }
    public string Description { get; init; } = string.Empty;

}
