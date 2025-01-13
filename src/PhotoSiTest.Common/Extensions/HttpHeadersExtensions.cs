using System.Net.Http.Headers;

namespace PhotoSiTest.Common.Extensions;

public static class HttpHeadersExtensions
{
    public static string Flatten(this HttpHeaders headers)
    {
        return !headers.Any() ? "none" : string.Join(',', headers.Select(h => $"['{h.Key}' => '{string.Join(", ", h.Value)}']"));
    }


    public static bool IsTextBasedContentType(this HttpHeaders? headers)
    {
        if (headers == null)
        {
            return false;
        }

        if (!headers.TryGetValues("Content-Type", out var values))
        {
            return false;
        }

        var header = string.Join(" ", values).ToLowerInvariant();

        var contentTypes = new[] { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

        return contentTypes.Any(header.Contains);
    }
}
