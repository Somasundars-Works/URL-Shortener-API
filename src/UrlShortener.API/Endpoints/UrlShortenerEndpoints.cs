namespace UrlShortener.API.Endpoints;

public static class UrlShortenerEndpoints
{
    public static WebApplication MapUrlShortenerEndpoints(this WebApplication app)
    {
        app.MapGet("/api/urls", UrlShortenerEndpointHandlers.GetAllUrlsAsync)
        .WithName("GetAllUrls")
        .WithSummary("Get all shortened URLs")
        .WithDescription("Returns the shortened URLs stored by the service.");

        app.MapGet("/api/urls/{id:guid}", UrlShortenerEndpointHandlers.GetUrlByIdAsync)
        .WithName("GetUrlById")
        .WithSummary("Get a shortened URL by ID")
        .WithDescription("Returns a shortened URL and its details for the specified ID.");

        app.MapGet("/api/urls/short/{shortenedUrl}", UrlShortenerEndpointHandlers.ResolveShortenedUrlAsync)
        .WithName("ResolveShortenedUrl")
        .WithSummary("Redirect to the original URL")
        .WithDescription("Resolves an active short code and redirects to its original URL.");

        app.MapPost("/api/urls", UrlShortenerEndpointHandlers.CreateUrlAsync)
        .WithName("CreateUrl")
        .WithSummary("Create a shortened URL")
        .WithDescription("Creates a short code for the supplied HTTP or HTTPS URL.");

        app.MapPut("/api/urls/{id:guid}", UrlShortenerEndpointHandlers.UpdateUrlAsync)
        .WithName("UpdateUrl")
        .WithSummary("Update a shortened URL")
        .WithDescription("Updates the destination, expiration date, and active status for a URL.");

        return app;
    }
}
