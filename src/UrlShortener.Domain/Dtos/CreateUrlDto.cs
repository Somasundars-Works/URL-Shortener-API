using System;

namespace UrlShortener.Domain.Dtos;

public class CreateUrlDto
{
    public string OriginalUrl { get; set; } = null!;
    public DateTime? ExpirationDate { get; set; }
}
