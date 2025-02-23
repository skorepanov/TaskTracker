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

    public async Task<IReadOnlyList<UserTask>> GetNonDeletedTasks()
    {
        return await _db.Tasks.Where(t => t.DeletionDate == null).ToListAsync();
    }

    public async Task<IReadOnlyList<UserTask>> GetTasksInTrash()
    {
        return await _db.Tasks.Where(t => t.DeletionDate != null).ToListAsync();
    }

    public async Task CreateTask(UserTask task, int? folderId)
    {
        _db.Tasks.Add(task);
        _db.Entry(task).Property("FolderId").CurrentValue = folderId;
        await _db.SaveChangesAsync();
    }

    public async Task UpdateTask(UserTask task)
    {
        _db.Tasks.Update(task);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateTaskWithFolder(UserTask task, int? folderId)
    {
        _db.Tasks.Update(task);
        _db.Entry(task).Property("FolderId").CurrentValue = folderId;
        await _db.SaveChangesAsync();
    }

    public async Task DeleteTaskPermanently(UserTask task)
    {
        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
    }
}
