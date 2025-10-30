namespace Application.DTOs.Pagination;

public sealed class PagedRequest
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}
