using System;

namespace UrlShortener.Domain;

public class Result<T>
{
    public T Data { get; set; } = default!;
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
}
