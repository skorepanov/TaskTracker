using Microsoft.EntityFrameworkCore;
using TaskTracker.Application.Interfaces;
using TaskTracker.Infrastructure;
using TaskTracker.Infrastructure.Repositories;
using TaskTracker.Web.HealthChecks;

namespace TaskTracker.Web;

/// <summary>
/// Расширения для Dependency Injection
/// </summary>
public static class DiExtensions
{
    extension(IServiceCollection collection)
    {
        /// <summary>
        /// Зарегистрировать сервисы
        /// </summary>
        public void AddServices()
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
        public void AddRepositories()
        {
            collection.AddScoped<ITaskRepository, TaskRepository>();
            collection.AddScoped<IFolderRepository, FolderRepository>();
            collection.AddScoped<ITagRepository, TagRepository>();
        }

        /// <summary>
        /// Сконфигурировать контекст БД
        /// </summary>
        public void ConfigureDbContext(string connectionString)
        {
            collection.AddDbContext<ApplicationContext>(
                options => options
                    .UseNpgsql(connectionString)
                    .UseSnakeCaseNamingConvention());
        }

        /// <summary>
        /// Зарегистрировать Health Checks
        /// </summary>
        public void AddCustomHealthChecks()
        {
            collection.AddHealthChecks()
                .AddCheck<DataBaseHealthCheck>(name: nameof(DataBaseHealthCheck));
        }
    }
}
