using System.Diagnostics;

namespace WIMS.Api.Middlewares;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(
        RequestDelegate next,
        ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            _logger.LogInformation(
                "Incoming Request: {Method} {Path}",
                context.Request.Method,
                context.Request.Path);


            _logger.LogInformation(
                "RequestId: {RequestId}",
                context.TraceIdentifier);

            await _next(context);

            stopwatch.Stop();

            _logger.LogInformation(
                "Outgoing Response: {StatusCode} completed in {ElapsedMilliseconds} ms",
                context.Response.StatusCode,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Unhandled Exception after {ElapsedMilliseconds} ms",
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}
