using Microsoft.AspNetCore.Diagnostics;

namespace TaskTracker.Web.Middleware;

/// <summary>
/// Обработчик исключений приложения
/// </summary>
public class ExceptionHandler : IExceptionHandler
{
    /// <summary>
    /// Обработать исключение
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var result = Result.Failure(exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        await httpContext.Response.WriteAsJsonAsync(result, cancellationToken);

        return true;
    }
}
