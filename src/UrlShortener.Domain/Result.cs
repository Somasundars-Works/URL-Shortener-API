using System;

namespace UrlShortener.Domain;

public class Result<T>
{
    public T Data { get; set; } = default!;
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;

    public static Result<T> Success(T data, string message = "")
    {
        return new Result<T>
        {
            Data = data,
            IsSuccess = true,
            Message = message
        };
    }

    public static Result<T> Failure(string message)
    {
        return new Result<T>
        {
            IsSuccess = false,
            Message = message
        };
    }
}
