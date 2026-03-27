using Microsoft.AspNetCore.Diagnostics;
using TaskTracker.Domain.Exceptions;

namespace TaskTracker.Web.Handlers;

/// <summary>
/// Глобальный обработчик исключений
/// </summary>
public class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    IHostEnvironment hostEnvironment)
    : IExceptionHandler
{
    /// <summary>
    /// Попытаться обработать исключение
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            DomainException => (StatusCodes.Status400BadRequest, "Bad Request"),
            _ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
        };

        httpContext.Response.StatusCode = status;

        var problemDetails = new ProblemDetails
        {
            Status = status,
            Title = title,
            Type = exception.GetType().Name,
            Detail = exception.Message,
            Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            Extensions =
            {
                ["timestamp"] = DateTime.UtcNow
            }
        };

        var isDevelopment = hostEnvironment.IsDevelopment();

        if (isDevelopment)
        {
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;
        }

        var problemDetailsContext = new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        };

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
