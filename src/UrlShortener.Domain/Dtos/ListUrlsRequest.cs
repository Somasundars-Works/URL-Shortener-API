using System;

namespace UrlShortener.Domain.Dtos;

public class ListUrlsRequest
{
    public Page Page { get; set; } = new Page();
    public Sort Sort { get; set; } = new Sort();
    public string? Search { get; set; }
    public bool ShowInactive { get; set; } = false;
}

public class Page
{
    public int Pn { get; set; } = 1;
    public int Ps { get; set; } = 10;
}

public class Sort
{
    public string SortBy { get; set; } = "CreatedAt";
    public string SortOrder { get; set; } = "desc";
}

public class ListUrlsResponse
{
    public List<GetByIdResponseDto> Urls { get; set; } = new List<GetByIdResponseDto>();
    public int TotalCount { get; set; }
}
