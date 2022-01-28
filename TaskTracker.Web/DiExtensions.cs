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
}
