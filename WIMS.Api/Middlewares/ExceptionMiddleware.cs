using System.Text.Json;
using WIMS.Application.DTOs;

namespace WIMS.Api.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception occurred. TraceId: {TraceId}",
                context.TraceIdentifier);

            await HandleExceptionAsync(context, ex);
        }
    }
    private static async Task HandleExceptionAsync(
    HttpContext context,
    Exception exception)
    {
        context.Response.ContentType = "application/json";

        int statusCode;
        string message;

        switch (exception)
        {
            case UnauthorizedAccessException:
                statusCode = StatusCodes.Status401Unauthorized;
                message = "Unauthorized access.";
                break;

            case ArgumentException:
                statusCode = StatusCodes.Status400BadRequest;
                message = "Invalid request.";
                break;

            case KeyNotFoundException:
                statusCode = StatusCodes.Status404NotFound;
                message = "Resource not found.";
                break;

            default:
                statusCode = StatusCodes.Status500InternalServerError;
                message = "Internal server error.";
                break;
        }

        context.Response.StatusCode = statusCode;

        var response = ApiResponse<object>.Failure(
            message,
            new List<string>
            {
            exception.Message
            },
            statusCode);

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}