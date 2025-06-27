using Microsoft.AspNetCore.Diagnostics;
using TaskTracker.Web.Models;

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
        var error = ApiResponse<object>.Failure(exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status200OK;
        await httpContext.Response.WriteAsJsonAsync(error, cancellationToken);

        return true;
    }
}
