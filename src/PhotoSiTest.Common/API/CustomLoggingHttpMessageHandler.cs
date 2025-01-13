using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PhotoSiTest.Common.Extensions;

namespace PhotoSiTest.Common.API;

public class CustomLoggingHttpMessageHandler : DelegatingHandler
{
    private readonly ILogger<CustomLoggingHttpMessageHandler> _logger;

    private readonly bool _logRequest;

    private readonly bool _logResponse;


    public CustomLoggingHttpMessageHandler(ILogger<CustomLoggingHttpMessageHandler> logger, bool logRequest, bool logResponse, bool useDependencyInjection = true)
    {
        _logger = logger;
        _logRequest = logRequest;
        _logResponse = logResponse;

        if (useDependencyInjection)
        {
            return;
        }

        InnerHandler = new HttpClientHandler();
    }


    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var requestStopwatch = await LogRequestAsync(request, ct);

        var response = await base.SendAsync(request, ct);

        await LogResponseAsync(response, requestStopwatch, ct);

        return response;
    }


    private static async Task<string> GetContent(HttpContent? content, HttpHeaders? headers, HttpHeaders? contentHeaders, CancellationToken ct)
    {
        switch (content)
        {
            case null: {
                return "null";
            }
            case JsonContent: {
                return await content.ReadAsStringAsync(ct);
            }
            default: {
                if (headers.IsTextBasedContentType() || contentHeaders.IsTextBasedContentType())
                {
                    return await content.ReadAsStringAsync(ct);
                }

                return "null";
            }
        }
    }


    private async Task<Stopwatch?> LogRequestAsync(HttpRequestMessage request, CancellationToken ct)
    {
        if (!_logRequest)
        {
            return null;
        }

        if (!_logger.IsEnabled(LogLevel.Debug))
        {
            return null;
        }

        var requestContent = await GetContent(request.Content, request.Headers, request.Content?.Headers, ct);

        _logger.LogDebug("""

                         ______ Request started ______
                         Request                : {RequestHTTPMethod} {RequestUri}
                         Request headers        : {RequestHeaders}
                         Request content headers: {RequestContentHeaders}
                         Request content        : {RequestContent}
                         """,
            request.Method,
            request.RequestUri?.ToString(),
            request.Headers.Flatten(),
            request.Content?.Headers.Flatten(),
            requestContent);

        return Stopwatch.StartNew();
    }


    private async Task LogResponseAsync(HttpResponseMessage response, Stopwatch? requestStopwatch, CancellationToken ct)
    {
        if (!_logResponse)
        {
            return;
        }

        if (!_logger.IsEnabled(LogLevel.Debug))
        {
            return;
        }

        var responseContent = await GetContent(response.Content, response.Headers, response.Content.Headers, ct);

        _logger.LogDebug("""

                         Request duration           : {TimeTaken}
                         Response status            : {ResponseStatusCode} ( reason {ResponseReason})
                         Response headers           : {ResponseHeaders}
                         Response content headers   : {ResponseContentHeaders}
                         Response content           : {ResponseContent}
                         ______ End ______
                         """,
            requestStopwatch?.Elapsed,
            (int)response.StatusCode,
            response.ReasonPhrase,
            response.Headers.Flatten(),
            response.Content.Headers.Flatten(),
            responseContent);
    }
}
