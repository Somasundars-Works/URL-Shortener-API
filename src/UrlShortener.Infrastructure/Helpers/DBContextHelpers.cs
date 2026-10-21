using System;
using System.Text;

namespace UrlShortener.Infrastructure.Helpers;

internal static class DBContextHelpers
{

    public static string GenerateShortenedCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var builder = new StringBuilder(8);

        for (var i = 0; i < 8; i++)
        {
            builder.Append(chars[random.Next(chars.Length)]);
        }

        return builder.ToString();
    }

    public static DateTime? NormalizeExpirationDate(DateTime? expirationDate)
    {
        if (expirationDate is null)
        {
            return null;
        }

        var value = expirationDate.Value;
        if (value.Kind == DateTimeKind.Unspecified)
        {
            value = DateTime.SpecifyKind(value, DateTimeKind.Utc);
        }

        return value.ToUniversalTime();
    }
}
