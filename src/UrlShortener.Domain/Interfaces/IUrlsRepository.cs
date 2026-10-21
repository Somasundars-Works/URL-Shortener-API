using System;
using UrlShortener.Domain;
using UrlShortener.Domain.Dtos;

namespace UrlShortener.Domain.Interfaces;

public interface IUrlsRepository
{
    Task<Result<string>> GetUrlByShortenedUrlAsync(string shortenedUrl, CancellationToken cancellationToken);
    Task<Result<GetByIdResponseDto>> GetUrlByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Result<ListUrlsResponse>> GetAllUrlsAsync(CancellationToken cancellationToken);
    Task<Result<string>> CreateUrlAsync(CreateUrlDto url, CancellationToken cancellationToken);
    Task<Result<string>> UpdateUrlAsync(Guid id, EditUrlDto url, CancellationToken cancellationToken);
}
