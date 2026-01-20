namespace TaskTracker.Infrastructure.Repositories;

public class TaskRepository(ApplicationContext db) : ITaskRepository
{
    public async Task<UserTask> GetTask(int taskId)
    {
        var task = await GetTasksWithTags().SingleOrDefaultAsync(t => t.Id == taskId);

        if (task is null)
        {
            var error = ErrorMessages.UserTaskNotFound(taskId);
            throw new DomainException(error);
        }

        return task;
    }

    public async Task<IReadOnlyList<UserTask>> GetIncompletedTasks()
    {
        return await GetTasksWithTags()
            .Where(t => t.MovedToTrashDateTime == null
                     && t.CompletedDateTime == null)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserTask>> GetCompletedTasks()
    {
        return await GetTasksWithTags()
            .Where(t => t.MovedToTrashDateTime == null
                     && t.CompletedDateTime != null)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserTask>> GetTasksInTrash()
    {
        return await GetTasksWithTags()
            .Where(t => t.MovedToTrashDateTime != null)
            .ToListAsync();
    }

    private IQueryable<UserTask> GetTasksWithTags()
    {
        return db.Tasks.Include(t => t.Tags);
    }

    public async Task CreateTask(UserTask task)
    {
        db.Tasks.Add(task);
        await db.SaveChangesAsync();
    }

    public async Task UpdateTask(UserTask task)
    {
        db.Tasks.Update(task);
        await db.SaveChangesAsync();
    }

    public async Task DeleteTask(UserTask task)
    {
        db.Tasks.Remove(task);
        await db.SaveChangesAsync();
    }
}
