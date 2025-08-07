using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TaskTracker.Dal;
using TaskTracker.Web;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

configureServices(webApplicationBuilder);

var webApplication = webApplicationBuilder.Build();

await applyDatabaseMigrations(webApplication);
configureHttpRequestPipeline(webApplication);

webApplication.Run();
return;


void configureServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddServices();
    builder.Services.AddRepositories();

    var connectionString = builder.Configuration
        .GetConnectionString(name: "Default");

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new Exception(message: "Не найдена строка подключения");
    }

    builder.Services.ConfigureDbContext(connectionString);
    builder.Services.AddCustomHealthChecks();

    builder.Services.AddOpenApi(options =>
    {
        options.AddDocumentTransformer<TaskTrackerApiTransformer>();
    });
}

async Task applyDatabaseMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    await dbContext.Database.MigrateAsync();
}

void configureHttpRequestPipeline(WebApplication app)
{
    app.UseExceptionHandler(_ => { });

    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(url: "/openapi/v1.json", name: "OpenAPI v1");
    });
    app.MapScalarApiReference();

    app.UseHttpsRedirection();
    app.UseCors(corsPolicyBuilder
        => corsPolicyBuilder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    app.UseAuthorization();
    app.MapControllers();
    app.MapHealthChecks(pattern: "api/health");
}

/// <summary>
/// Класс Program для возможности его использования в интеграционных тестах
/// </summary>
public partial class Program;
