using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;
using TaskTracker.Dal;

namespace TaskTracker.Web;

public static class DiExtensions
{
    public static void AddServices(this IServiceCollection collection)
    {
        collection.AddTransient<TaskService>();
    }

    public static void AddRepositories(this IServiceCollection collection)
    {
        collection.AddTransient<ITaskRepository, TaskRepository>();
    }

    public static void ConfigureDbContext(this IServiceCollection collection,
                                          string connectionString)
    {
        var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var sqlitePath = Path.Combine(folderPath, connectionString);

        collection.AddDbContext<ApplicationContext>(
            options => options.UseSqlite(sqlitePath));
    }
}
