using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Authentication;

namespace Qorpe.Api.Handlers;

/// <summary>
/// Handles global exceptions in the application and logs details for debugging purposes.
/// </summary>
/// <param name="logger"></param>
public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    /// <summary>
    /// Tries to handle an exception by logging details and returning an appropriate problem response.
    /// </summary>
    /// <param name="httpContext">The HTTP context of the current request.</param>
    /// <param name="exception">The exception to handle.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="ValueTask"/> representing the asynchronous operation, indicating whether the exception was handled.</returns>
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        // Retrieve the trace identifier for the request
        var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

        // Log exception details for debugging purposes
        logger.LogError(
            exception,
            "Could not process a request on machine {MachineName}. TraceId: {TraceId}",
            Environment.MachineName,
            traceId
        );

        // Map the exception to status code, title, and detail
        (int statusCode, string? title, string? detail) = MapException(exception);

        Dictionary<string, object?> extensions = new()
        {
            { "traceId", traceId }
        };

        // Return a problem response with the mapped details
        await Results.Problem(
            type: exception.GetType().Name,
            title: title,
            detail: detail,
            statusCode: statusCode,
            instance: $"{httpContext.Request.Method} {httpContext.Request.Path}",
            extensions: extensions
        ).ExecuteAsync(httpContext);

        return true;
    }

    private static (int statusCode, string? title, string? detail) MapException(Exception? exception)
    {
        return exception switch
        {
            // 400 Bad Request
            ArgumentNullException or ArgumentException or ApplicationException or
            InvalidOperationException or FormatException or DivideByZeroException =>
                (StatusCodes.Status400BadRequest, "Bad Request", "An error occurred with the request."),

            // 401 Unauthorized
            UnauthorizedAccessException or AuthenticationException /*or TokenExpiredException*/ =>
                (StatusCodes.Status401Unauthorized, "Unauthorized", "Access to the requested resource is denied."),

            // 403 Forbidden
            // ForbiddenException =>
                // (StatusCodes.Status403Forbidden, "Forbidden", "You do not have permission to access this resource."),

            // 404 Not Found
            KeyNotFoundException or FileNotFoundException or DirectoryNotFoundException =>
                (StatusCodes.Status404NotFound, "Not Found", "The requested resource was not found."),

            // 408 Request Timeout
            TimeoutException or TaskCanceledException =>
                (StatusCodes.Status408RequestTimeout, "Request Timeout", "The request timed out."),

            // 409 Conflict
            // DbUpdateConcurrencyException =>
                // (StatusCodes.Status409Conflict, "Conflict", "A concurrency error occurred during database update."),

            // 422 Unprocessable Entity
            ValidationException =>
                (StatusCodes.Status422UnprocessableEntity, "Unprocessable Entity", "The request was well-formed but contains semantic errors."),

            // 501 Not Implemented
            NotImplementedException =>
                (StatusCodes.Status501NotImplemented, "Not Implemented", "The requested method is not implemented."),

            // 503 Service Unavailable
            HttpRequestException or ExternalException =>
                (StatusCodes.Status503ServiceUnavailable, "Service Unavailable", "The service is temporarily unavailable."),

            // 500 Internal Server Error
            StackOverflowException or OutOfMemoryException =>
                (StatusCodes.Status500InternalServerError, "Internal Server Error", "A critical system error occurred."),

            _ =>
                (StatusCodes.Status500InternalServerError, "Internal Server Error", "An unexpected error occurred.")
        };
    }
}
