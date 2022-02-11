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
        // TODO use real DB
        var completedUserTaskChangeData = new UserTaskChangeData
        {
            Id = 3,
            Title = "Completed task",
            Description = "Description",
        };
        var completedTask = UserTask.CreateTask(completedUserTaskChangeData);
        completedTask.Complete(DateTime.Now);

        var incompleteUserTaskChangeData = new UserTaskChangeData
        {
            Id = 4,
            Title = "Overdue task",
            Description = "Description",
        };
        var incompleteTask = UserTask.CreateTask(incompleteUserTaskChangeData);
        incompleteTask.DueDate = DateTime.Now.AddDays(-1);

        var tasks = new List<UserTask> { completedTask, incompleteTask };
        return tasks;
    }

    public async Task<IReadOnlyCollection<UserTask>> GetDeletedTasks()
    {
        // TODO use real DB
        var changeData = new UserTaskChangeData
        {
            Id = 66,
            Title = "Deleted task",
            Description = "Description",
        };
        var deletedTask = UserTask.CreateTask(changeData);
        deletedTask.Delete(DateTime.Now.AddDays(-1));
        var tasks = new List<UserTask> { deletedTask };
        return tasks;
    }

    public async Task<Folder> GetFolder(int folderId)
    {
        return await _db.Folders.FindAsync(folderId);
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
