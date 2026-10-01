using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain;

public class UrlsTable
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string OriginalUrl { get; set; } = null!;

    [Required]
    public string ShortenedUrl { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public bool IsActive { get; set; }
}
