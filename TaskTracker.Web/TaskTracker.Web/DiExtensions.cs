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
        collection.AddTransient<ExecutionService>();
        collection.AddTransient<TaskService>();
        collection.AddTransient<FolderService>();
        collection.AddTransient<TagService>();
    }

    /// <summary>
    /// Зарегистрировать репозитории
    /// </summary>
    public static void AddRepositories(this IServiceCollection collection)
    {
        collection.AddTransient<ITaskRepository, TaskRepository>();
        collection.AddTransient<IFolderRepository, FolderRepository>();
        collection.AddTransient<ITagRepository, TagRepository>();
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
