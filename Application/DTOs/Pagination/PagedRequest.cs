using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Pagination;

public sealed class PagedRequest
{
    [Range(1, int.MaxValue)]
    public int Page { get; init; } = 1;

    [Range(1, 200)]
    public int PageSize { get; init; } = 20;
}
