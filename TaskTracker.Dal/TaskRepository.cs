using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;

namespace TaskTracker.Dal;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationContext _db;

    public TaskRepository(ApplicationContext db)
    {
        this._db = db;
    }

    public async Task<UserTask> GetTask(int taskId)
    {
        return await this._db.Tasks.FindAsync(taskId);
    }

    public async Task<IReadOnlyCollection<UserTask>> GetNonDeletedTasks()
    {
        return await _db.Tasks.Where(t => t.DeletionDate == null).ToListAsync();
    }

    public async Task<IReadOnlyCollection<UserTask>> GetDeletedTasks()
    {
        return await _db.Tasks.Where(t => t.DeletionDate != null).ToListAsync();
    }

    public async Task<Folder> GetFolder(int folderId)
    {
        return await _db.Folders
            .Include(f => f.Tasks)
            .SingleOrDefaultAsync(f => f.Id == folderId);
    }

    public async Task<IReadOnlyCollection<Folder>> GetFolders()
    {
        return await _db.Folders.Include(f => f.Tasks).ToListAsync();
    }

    public async Task SaveNewTask(UserTask task, int folderId)
    {
        _db.Tasks.Add(task);
        _db.Entry(task).Property("FolderId").CurrentValue = folderId;
        await _db.SaveChangesAsync();
    }

    public async Task SaveNewFolder(Folder folder)
    {
        _db.Folders.Add(folder);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateTask(UserTask task)
    {
        throw new System.NotImplementedException();
    }

    public async Task UpdateTaskFolder(int taskId, int folderId)
    {
        throw new System.NotImplementedException();
    }
}
