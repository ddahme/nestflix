using System.ComponentModel;

namespace Api.Pagination;

public sealed record PageRequestDto
{
    [DefaultValue(1)]
    public int Page { get; set; }
    [DefaultValue(10)]
    public int Size { get; set; }

    // Keeping existing method name to avoid breaking callers
    public int Offest() => (Page - 1) * Size;
}
