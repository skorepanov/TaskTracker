using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll.Interfaces;
using TaskTracker.Dal;
using TaskTracker.Dal.Repositories;
using TaskTracker.Web.HealthChecks;

namespace TaskTracker.Web;

/// <summary>
/// Расширения для Dependency Injection
/// </summary>
public static class DiExtensions
{
    /// <summary>
    /// Зарегистрировать сервисы
    /// </summary>
    public static void AddServices(this IServiceCollection collection)
    {
        collection.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        collection.AddScoped<ExecutionService>();
        collection.AddScoped<TaskService>();
        collection.AddScoped<FolderService>();
        collection.AddScoped<TagService>();
    }

    /// <summary>
    /// Зарегистрировать репозитории
    /// </summary>
    public static void AddRepositories(this IServiceCollection collection)
    {
        collection.AddScoped<ITaskRepository, TaskRepository>();
        collection.AddScoped<IFolderRepository, FolderRepository>();
        collection.AddScoped<ITagRepository, TagRepository>();
    }

    /// <summary>
    /// Сконфигурировать контекст БД
    /// </summary>
    public static void ConfigureDbContext(
        this IServiceCollection collection,
        string connectionString)
    {
        collection.AddDbContext<ApplicationContext>(
            options => options
                .UseNpgsql(connectionString)
                .UseSnakeCaseNamingConvention());
    }

    /// <summary>
    /// Зарегистрировать Health Checks
    /// </summary>
    public static void AddCustomHealthChecks(this IServiceCollection collection)
    {
        collection.AddHealthChecks()
            .AddCheck<DataBaseHealthCheck>(name: nameof(DataBaseHealthCheck));
    }
}
