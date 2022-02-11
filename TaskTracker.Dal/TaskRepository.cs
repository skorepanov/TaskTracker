using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;

namespace TaskTracker.Dal;

// TODO use real DB
public class TaskRepository : ITaskRepository
{
    private readonly ApplicationContext _db;

    public TaskRepository(ApplicationContext db)
    {
        this._db = db;
    }

    public UserTask GetTask(int taskId)
    {
        return this._db.Tasks.Find(taskId);
    }

    public IReadOnlyCollection<UserTask> GetNonDeletedTasks()
    {
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

    public IReadOnlyCollection<UserTask> GetDeletedTasks()
    {
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

    public Folder GetFolder(int folderId)
    {
        return _db.Folders.Find(folderId);
    }

    public IReadOnlyCollection<Folder> GetFolders()
    {
        return _db.Folders.Include(f => f.Tasks).ToList();
    }

    public void SaveNewTask(UserTask task, int folderId)
    {
        _db.Tasks.Add(task);
        _db.Entry(task).Property("FolderId").CurrentValue = folderId;
        _db.SaveChanges();
    }

    public void SaveNewFolder(Folder folder)
    {
        _db.Folders.Add(folder);
        _db.SaveChanges();
    }

    public void UpdateTask(UserTask task)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateTaskFolder(int taskId, int folderId)
    {
        throw new System.NotImplementedException();
    }
}
