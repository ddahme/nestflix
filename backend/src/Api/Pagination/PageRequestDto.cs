namespace Api.Pagination;

public sealed record PageRequestDto
{
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 10;

    public int Offest() => (Page - 1) * Size;
}
