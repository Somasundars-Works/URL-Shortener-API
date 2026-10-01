using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Domain;

public class UrlsClicksAudit
{
    [Key]
    public Guid Id { get; set; }

    public Guid UrlId { get; set; }

    public DateTime ClickedAt { get; set; }

    public string? UserAgent { get; set; }

    public string? IpAddress { get; set; }

    public string? Referrer { get; set; }
}
