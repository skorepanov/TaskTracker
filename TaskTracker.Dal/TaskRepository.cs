using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;
using TaskTracker.Bll.Models;

namespace TaskTracker.Dal;

public class TaskRepository(ApplicationContext _db) : ITaskRepository
{
    public async Task<UserTask?> GetTask(int taskId)
    {
        return await GetTasksWithTags()
            .SingleOrDefaultAsync(t => t.Id == taskId);
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
        return _db.Tasks.Include(t => t.Tags);
    }

    public async Task CreateTask(UserTask task)
    {
        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateTask(UserTask task)
    {
        _db.Tasks.Update(task);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteTask(UserTask task)
    {
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
    }
}
