using System;

namespace UrlShortener.Domain.Dtos;

public class EditUrlDto
{
    public string OriginalUrl { get; set; } = null!;
    public DateTime? ExpirationDate { get; set; }
    public bool IsActive { get; set; }
}
