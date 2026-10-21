using System;
using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using UrlShortener.Domain;
using UrlShortener.Domain.Dtos;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Infrastructure.Helpers;

namespace UrlShortener.Infrastructure.Repository;

public class UrlsRepository : IUrlsRepository
{
    private readonly IDynamoDBContext _dynamoDbContext;

    public UrlsRepository(IDynamoDBContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }

    public async Task<Result<string>> CreateUrlAsync(CreateUrlDto url, CancellationToken cancellationToken)
    {
        if (url is null)
        {
            return Result<string>.Failure("CreateUrl request is required.");
        }

        if (string.IsNullOrWhiteSpace(url.OriginalUrl))
        {
            return Result<string>.Failure("OriginalUrl is required.");
        }

        var originalUrl = url.OriginalUrl.Trim();
        if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return Result<string>.Failure("OriginalUrl must be a valid absolute HTTP or HTTPS URL.");
        }

        var item = new UrlsTable
        {
            Id = Guid.NewGuid(),
            OriginalUrl = originalUrl,
            ShortenedUrl = await GenerateUniqueShortenedUrlAsync(cancellationToken),
            CreatedAt = DateTime.UtcNow,
            ExpirationDate = DBContextHelpers.NormalizeExpirationDate(url.ExpirationDate),
            IsActive = true
        };

        await _dynamoDbContext.SaveAsync(item, cancellationToken);

        return new Result<string>
        {
            IsSuccess = true,
            Message = "URL created successfully.",
            Data = item.ShortenedUrl
        };
    }

    public async Task<Result<ListUrlsResponse>> GetAllUrlsAsync(CancellationToken cancellationToken)
    {
        var items = await _dynamoDbContext
            .ScanAsync<UrlsTable>(new List<ScanCondition>())
            .GetRemainingAsync(cancellationToken);

        var response = new ListUrlsResponse
        {
            Urls = items
                .Select(item => new GetByIdResponseDto
                {
                    OriginalUrl = item.OriginalUrl,
                    ShortenedUrl = item.ShortenedUrl,
                    ExpirationDate = item.ExpirationDate,
                    IsActive = item.IsActive,
                    UrlAudits = []
                })
                .ToList(),
            TotalCount = items.Count
        };

        return new Result<ListUrlsResponse>
        {
            IsSuccess = true,
            Message = "URLs retrieved successfully.",
            Data = response
        };
    }

    public async Task<Result<GetByIdResponseDto>> GetUrlByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var item = await _dynamoDbContext.LoadAsync<UrlsTable>(id, cancellationToken);
        if (item is null)
        {
            return Result<GetByIdResponseDto>.Failure($"URL with id {id} was not found.");
        }

        return new Result<GetByIdResponseDto>
        {
            IsSuccess = true,
            Message = "URL retrieved successfully.",
            Data = new GetByIdResponseDto
            {
                OriginalUrl = item.OriginalUrl,
                ShortenedUrl = item.ShortenedUrl,
                ExpirationDate = item.ExpirationDate,
                IsActive = item.IsActive,
                UrlAudits = []
            }
        };
    }

    public async Task<Result<string>> GetUrlByShortenedUrlAsync(string shortenedUrl, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(shortenedUrl))
        {
            return Result<string>.Failure("ShortenedUrl is required.");
        }

        var normalizedShortUrl = shortenedUrl.Trim();
        var result = await _dynamoDbContext
            .ScanAsync<UrlsTable>(new List<ScanCondition>
            {
                new(nameof(UrlsTable.ShortenedUrl), ScanOperator.Equal, normalizedShortUrl)
            })
            .GetRemainingAsync(cancellationToken);

        var item = result
            .FirstOrDefault(x => x.IsActive && (x.ExpirationDate is null || x.ExpirationDate > DateTime.UtcNow));

        if (item is null)
        {
            return Result<string>.Failure("URL not found.");
        }

        return new Result<string>
        {
            IsSuccess = true,
            Message = "URL found.",
            Data = item.OriginalUrl
        };
    }

    public async Task<Result<string>> UpdateUrlAsync(Guid id, EditUrlDto url, CancellationToken cancellationToken)
    {
        if (url is null)
        {
            return Result<string>.Failure("UpdateUrl request is required.");
        }

        if (string.IsNullOrWhiteSpace(url.OriginalUrl))
        {
            return Result<string>.Failure("OriginalUrl is required.");
        }

        var existing = await _dynamoDbContext.LoadAsync<UrlsTable>(id, cancellationToken);
        if (existing is null)
        {
            return Result<string>.Failure($"URL with id {id} was not found.");
        }

        var originalUrl = url.OriginalUrl.Trim();
        if (!Uri.TryCreate(originalUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            return Result<string>.Failure("OriginalUrl must be a valid absolute HTTP or HTTPS URL.");
        }

        existing.OriginalUrl = originalUrl;
        existing.ExpirationDate = DBContextHelpers.NormalizeExpirationDate(url.ExpirationDate);
        existing.IsActive = url.IsActive;

        await _dynamoDbContext.SaveAsync(existing, cancellationToken);

        return new Result<string>
        {
            IsSuccess = true,
            Message = "URL updated successfully.",
            Data = existing.ShortenedUrl
        };
    }

    private async Task<string> GenerateUniqueShortenedUrlAsync(CancellationToken cancellationToken)
    {
        while (true)
        {
            var candidate = DBContextHelpers.GenerateShortenedCode();
            var matches = await _dynamoDbContext
                .ScanAsync<UrlsTable>(new List<ScanCondition>
                {
                    new(nameof(UrlsTable.ShortenedUrl), ScanOperator.Equal, candidate)
                })
                .GetRemainingAsync(cancellationToken);

            if (matches.Count == 0)
            {
                return candidate;
            }
        }
    }

}
