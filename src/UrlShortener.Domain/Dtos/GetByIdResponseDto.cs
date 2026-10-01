using System;

namespace UrlShortener.Domain.Dtos;

public class GetByIdResponseDto
{
    public string OriginalUrl { get; set; } = null!;
    public string ShortenedUrl { get; set; } = null!;
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; }

    public List<UrlAuditsDto> UrlAudits { get; set; } = [];
}

public class UrlAuditsDto
{
    public DateTime AccessedAt { get; set; }
    public string AccessedByIp { get; set; } = null!;
    public string UserAgent { get; set; } = null!;
    public string Referrer { get; set; } = null!;
}