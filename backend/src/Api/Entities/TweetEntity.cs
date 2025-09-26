using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities;

public class TweetEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid BoxId { get; set; }
    public string BlobName { get; set; }
    public DateTime UploadedAt { get; set; }
    public bool? IsOccupied { get; set; }
    public string? BirdType { get; set; }
    public int? EggCount { get; set; }
    public int? HatchedCount { get; set; }
    public int? DeadCount { get; set; }
    public string Description { get; set; } = string.Empty;

    public virtual BoxEntity Box { get; set; } = null!;
}
