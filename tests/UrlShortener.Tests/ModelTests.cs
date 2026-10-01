using UrlShortener.Domain;

namespace UrlShortener.Tests;

public class ModelTests
{
    [Fact]
    public void UrlsTable_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var url = new UrlsTable();

        // Assert
        Assert.Equal(Guid.Empty, url.Id);
        Assert.Null(url.OriginalUrl);
        Assert.Null(url.ShortenedUrl);
        Assert.Equal(default(DateTime), url.CreatedAt);
        Assert.Null(url.ExpirationDate);
        Assert.False(url.IsActive);
    }

    [Fact]
    public void UrlsClicksAudit_InitializesWithDefaultValues()
    {
        // Arrange & Act
        var audit = new UrlsClicksAudit();

        // Assert
        Assert.Equal(Guid.Empty, audit.Id);
        Assert.Equal(Guid.Empty, audit.UrlId);
        Assert.Equal(default(DateTime), audit.ClickedAt);
        Assert.Null(audit.UserAgent);
        Assert.Null(audit.IpAddress);
        Assert.Null(audit.Referrer);
    }

    [Fact]
    public void UrlsTable_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var url = new UrlsTable
        {
            Id = Guid.NewGuid(),
            OriginalUrl = "https://example.com",
            ShortenedUrl = "https://short.ly/abc123",
            CreatedAt = DateTime.UtcNow,
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            IsActive = true
        };

        // Act & Assert
        Assert.Equal("https://example.com", url.OriginalUrl);
        Assert.Equal("https://short.ly/abc123", url.ShortenedUrl);
        Assert.Equal(DateTime.UtcNow.Date, url.CreatedAt.Date);
        Assert.Equal(DateTime.UtcNow.AddDays(7).Date, url.ExpirationDate?.Date);
        Assert.True(url.IsActive);
    }

    [Fact]
    public void UrlsClicksAudit_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var audit = new UrlsClicksAudit
        {
            Id = Guid.NewGuid(),
            UrlId = Guid.NewGuid(),
            ClickedAt = DateTime.UtcNow,
            UserAgent = "Mozilla/5.0",
            IpAddress = "192.168.1.1",
            Referrer = "https://referrer.com"
        };

        // Act & Assert
        Assert.Equal("Mozilla/5.0", audit.UserAgent);
        Assert.Equal("192.168.1.1", audit.IpAddress);
        Assert.Equal("https://referrer.com", audit.Referrer);
    }
}
