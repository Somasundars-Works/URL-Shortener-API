using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using UrlShortener.API.Endpoints;
using UrlShortener.Domain.Interfaces;
using UrlShortener.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add DynamoDB service to the container
builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddSingleton<IDynamoDBContext, DynamoDBContext>();
builder.Services.AddScoped<IUrlsRepository, UrlsRepository>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapUrlShortenerEndpoints();

app.MapGet("/api/health", () =>
{
    return Results.Ok(new { status = "Healthy" });
})
.WithName("HealthCheck");

app.Run();
