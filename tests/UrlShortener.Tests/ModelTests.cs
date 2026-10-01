using UrlShortener.Domain;
using UrlShortener.Domain.Dtos;

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

    [Fact]
    public void EditUrlDto_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var editUrlDto = new EditUrlDto
        {
            OriginalUrl = "https://example.com",
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            IsActive = true
        };

        // Act & Assert
        Assert.Equal("https://example.com", editUrlDto.OriginalUrl);
        Assert.Equal(DateTime.UtcNow.AddDays(7).Date, editUrlDto.ExpirationDate?.Date);
        Assert.True(editUrlDto.IsActive);
    }

    [Fact]
    public void GetByIdResponseDto_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var getByIdResponseDto = new GetByIdResponseDto
        {
            OriginalUrl = "https://example.com",
            ShortenedUrl = "https://short.ly/abc123",
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            IsActive = true,
            UrlAudits = new List<UrlAuditsDto>
            {
                new UrlAuditsDto
                {
                    AccessedAt = DateTime.UtcNow,
                    AccessedByIp = "192.168.1.1",
                    UserAgent = "Mozilla/5.0",
                    Referrer = "https://referrer.com"
                }
            }
        };

        // Act & Assert
        Assert.Equal("https://example.com", getByIdResponseDto.OriginalUrl);
        Assert.Equal("https://short.ly/abc123", getByIdResponseDto.ShortenedUrl);
        Assert.Equal(DateTime.UtcNow.AddDays(7).Date, getByIdResponseDto.ExpirationDate?.Date);
        Assert.True(getByIdResponseDto.IsActive);
        Assert.Single(getByIdResponseDto.UrlAudits);
        Assert.Equal("192.168.1.1", getByIdResponseDto.UrlAudits[0].AccessedByIp);
        Assert.Equal("Mozilla/5.0", getByIdResponseDto.UrlAudits[0].UserAgent);
        Assert.Equal("https://referrer.com", getByIdResponseDto.UrlAudits[0].Referrer);
    }

    [Fact]
    public void UrlAuditsDto_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var urlAuditsDto = new UrlAuditsDto
        {
            AccessedAt = DateTime.UtcNow,
            AccessedByIp = "192.168.1.1",
            UserAgent = "Mozilla/5.0",
            Referrer = "https://referrer.com"
        };

        // Act & Assert
        Assert.Equal(DateTime.UtcNow.Date, urlAuditsDto.AccessedAt.Date);
        Assert.Equal("192.168.1.1", urlAuditsDto.AccessedByIp);
        Assert.Equal("Mozilla/5.0", urlAuditsDto.UserAgent);
        Assert.Equal("https://referrer.com", urlAuditsDto.Referrer);
    }

    [Fact]
    public void ListUrlsRequest_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var listUrlsRequest = new ListUrlsRequest
        {
            Page = new Page { Pn = 2, Ps = 20 },
            Sort = new Sort { SortBy = "OriginalUrl", SortOrder = "asc" },
            Search = "example",
            ShowInactive = true
        };

        // Act & Assert
        Assert.Equal(2, listUrlsRequest.Page.Pn);
        Assert.Equal(20, listUrlsRequest.Page.Ps);
        Assert.Equal("OriginalUrl", listUrlsRequest.Sort.SortBy);
        Assert.Equal("asc", listUrlsRequest.Sort.SortOrder);
        Assert.Equal("example", listUrlsRequest.Search);
        Assert.True(listUrlsRequest.ShowInactive);
    }

    [Fact]
    public void ListUrlsResponse_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var listUrlsResponse = new ListUrlsResponse
        {
            Urls = new List<GetByIdResponseDto>
            {
                new GetByIdResponseDto
                {
                    OriginalUrl = "https://example.com",
                    ShortenedUrl = "https://short.ly/abc123",
                    ExpirationDate = DateTime.UtcNow.AddDays(7),
                    IsActive = true
                }
            },
            TotalCount = 1
        };

        // Act & Assert
        Assert.Single(listUrlsResponse.Urls);
        Assert.Equal("https://example.com", listUrlsResponse.Urls[0].OriginalUrl);
        Assert.Equal("https://short.ly/abc123", listUrlsResponse.Urls[0].ShortenedUrl);
        Assert.Equal(DateTime.UtcNow.AddDays(7).Date, listUrlsResponse.Urls[0].ExpirationDate?.Date);
        Assert.True(listUrlsResponse.Urls[0].IsActive);
        Assert.Equal(1, listUrlsResponse.TotalCount);
    }

    [Fact]
    public void ResultDto_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var resultDto = new Result<string>
        {
            IsSuccess = true,
            Message = "Operation successful.",
            Data = "Sample data"
        };

        // Act & Assert
        Assert.True(resultDto.IsSuccess);
        Assert.Equal("Operation successful.", resultDto.Message);
        Assert.Equal("Sample data", resultDto.Data);
    }

    [Fact]
    public void CreateUrlDto_PropertiesCanBeSetAndRetrieved()
    {
        // Arrange
        var createUrlDto = new CreateUrlDto
        {
            OriginalUrl = "https://example.com",
            ExpirationDate = DateTime.UtcNow.AddDays(7),
        };

        // Act & Assert
        Assert.Equal("https://example.com", createUrlDto.OriginalUrl);
        Assert.Equal(DateTime.UtcNow.AddDays(7).Date, createUrlDto.ExpirationDate?.Date);
    }
}
