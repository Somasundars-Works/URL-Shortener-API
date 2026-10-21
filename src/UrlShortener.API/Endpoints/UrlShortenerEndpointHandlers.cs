using Microsoft.AspNetCore.Http.HttpResults;
using UrlShortener.Domain;
using UrlShortener.Domain.Dtos;
using UrlShortener.Domain.Interfaces;

namespace UrlShortener.API.Endpoints;

public static class UrlShortenerEndpointHandlers
{
    public static async Task<Results<Ok<Result<ListUrlsResponse>>, ProblemHttpResult>> GetAllUrlsAsync(
        IUrlsRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetAllUrlsAsync(cancellationToken);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Unable to retrieve URLs",
                detail: result.Message);
    }

    public static async Task<Results<Ok<Result<GetByIdResponseDto>>, ProblemHttpResult>> GetUrlByIdAsync(
        Guid id,
        IUrlsRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetUrlByIdAsync(id, cancellationToken);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "URL not found",
                detail: result.Message);
    }

    public static async Task<Results<RedirectHttpResult, ProblemHttpResult>> ResolveShortenedUrlAsync(
        string shortenedUrl,
        IUrlsRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.GetUrlByShortenedUrlAsync(shortenedUrl, cancellationToken);
        return result.IsSuccess
            ? TypedResults.Redirect(result.Data)
            : TypedResults.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Short URL not found",
                detail: result.Message);
    }

    public static async Task<Results<Created<Result<string>>, ProblemHttpResult>> CreateUrlAsync(
        CreateUrlDto request,
        IUrlsRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.CreateUrlAsync(request, cancellationToken);
        return result.IsSuccess
            ? TypedResults.Created($"/api/urls/short/{result.Data}", result)
            : TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid URL",
                detail: result.Message);
    }

    public static async Task<Results<Ok<Result<string>>, ProblemHttpResult>> UpdateUrlAsync(
        Guid id,
        EditUrlDto request,
        IUrlsRepository repository,
        CancellationToken cancellationToken)
    {
        var result = await repository.UpdateUrlAsync(id, request, cancellationToken);
        return result.IsSuccess
            ? TypedResults.Ok(result)
            : TypedResults.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Unable to update URL",
                detail: result.Message);
    }
}
