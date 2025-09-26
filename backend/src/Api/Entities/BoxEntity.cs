using Api.DTOs;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities;

public sealed class BoxEntity
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;
    public Point Point { get; set; } = null!;
    public BoxType BoxType { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsArchived { get; set; } = false;
    public Guid? LatestTweet { get; set; }
}
