namespace Api.Entities;

public sealed class TweetEntity
{
    public required Guid BoxId { get; init; }
    public required Uri SasUri { get; init; }
    public required DateTime UploadedAt { get; init; }
    public required bool IsOccupied { get; init; }
    public string? BirdType { get; init; }
    public int? EggCount { get; init; }
    public int? HatchedCount { get; init; }
    public int? DeadCount { get; init; }
    public required string Description { get; init; }
}
