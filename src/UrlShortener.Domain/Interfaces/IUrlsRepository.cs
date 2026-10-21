using System;
using UrlShortener.Domain;
using UrlShortener.Domain.Dtos;

namespace UrlShortener.Domain.Interfaces;

public interface IUrlsRepository
{
    public Task<Result<string>> GetUrlByShortenedUrlAsync(string shortenedUrl);
    public Task<Result<GetByIdResponseDto>> GetUrlByIdAsync(Guid id);
    public Task<Result<ListUrlsResponse>> GetAllUrlsAsync();
    public Task<Result<string>> CreateUrlAsync(CreateUrlDto url);
    public Task<Result<string>> UpdateUrlAsync(Guid id, EditUrlDto url);
}
