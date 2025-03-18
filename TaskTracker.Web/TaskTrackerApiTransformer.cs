using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TaskTracker.Web;

/// <summary>
/// Конфигуратор документации OpenAPI
/// </summary>
public class TaskTrackerApiTransformer : IOpenApiDocumentTransformer
{
    /// <summary>
    /// Сконфигурировать документацию OpenAPI
    /// </summary>
    public async Task TransformAsync(
        OpenApiDocument document,
        OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        document.Info = new OpenApiInfo
        {
            Title = "Task Tracker",
            Version = "0.0.1",
            Description = "TaskTracker API"
        };

        await Task.CompletedTask;
    }
}