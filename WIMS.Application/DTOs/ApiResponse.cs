namespace WIMS.Application.DTOs;

public class ApiResponse<T>
{
    public bool IsSuccess { get; set; }
    public required string Message { get; set; }
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
    public int? StatusCode { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Request successful.", int? statusCode = null)
        => new() { IsSuccess = true, Data = data, Message = message, StatusCode = statusCode };

    public static ApiResponse<T> Failure(string message, List<string>? errors = null, int? statusCode = null)
        => new() { IsSuccess = false, Message = message, Errors = errors, StatusCode = statusCode };

    public static ApiResponse<T> EmptyResponse(string message = "No data available.", int? statusCode = null)
        => new() { IsSuccess = true, Message = message, Data = default, StatusCode = statusCode };
}
