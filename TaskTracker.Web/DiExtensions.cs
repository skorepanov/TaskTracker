using Microsoft.EntityFrameworkCore;
using TaskTracker.Dal;

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
        collection.AddTransient<TaskService>();
    }

    /// <summary>
    /// Зарегистрировать репозитории
    /// </summary>
    public static void AddRepositories(this IServiceCollection collection)
    {
        collection.AddTransient<ITaskRepository, TaskRepository>();
    }

    /// <summary>
    /// Зарегистрировать контексты БД
    /// </summary>
    public static void ConfigureDbContext(this IServiceCollection collection,
                                          string connectionString)
    {
        collection.AddDbContext<ApplicationContext>(
            options => options.UseNpgsql(connectionString));
    }
}
