using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;
using TaskTracker.Bll.Models;

namespace TaskTracker.Dal;

public class TaskRepository(ApplicationContext _db) : ITaskRepository
{
    public async Task<UserTask?> GetTask(int taskId)
    {
        return await _db.Tasks.FindAsync(taskId);
    }

    public async Task<IReadOnlyList<UserTask>> GetIncompletedTasks()
    {
        return await _db.Tasks
            .Where(t => t.MovedToTrashDateTime == null
                     && t.CompletedDateTime == null)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserTask>> GetCompletedTasks()
    {
        return await _db.Tasks
            .Where(t => t.MovedToTrashDateTime == null
                     && t.CompletedDateTime != null)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserTask>> GetNonMovedToTrashTasks()
    {
        return await _db.Tasks
            .Where(t => t.MovedToTrashDateTime == null)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserTask>> GetTasksInInbox()
    {
        return await _db.Tasks
            .Where(t => t.FolderId == null && t.MovedToTrashDateTime == null)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<UserTask>> GetTasksInTrash()
    {
        return await _db.Tasks
            .Where(t => t.MovedToTrashDateTime != null)
            .ToListAsync();
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
