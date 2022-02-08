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
        var completedTask = new UserTask(id: 3, title: "Completed task", description: "Description");
        completedTask.Complete(DateTime.Now);

        var incompleteTask = new UserTask(id: 4, title: "Overdue task", description: "Description");
        incompleteTask.DueDate = DateTime.Now.AddDays(-1);

        var tasks = new List<UserTask> { completedTask, incompleteTask };
        return tasks;
    }

    public IReadOnlyCollection<UserTask> GetDeletedTasks()
    {
        var deletedTask = new UserTask(id: 66, title: "Deleted task", description: "Description");
        deletedTask.Delete(DateTime.Now.AddDays(-1));
        var tasks = new List<UserTask> { deletedTask };
        return tasks;
    }

    public Folder GetFolder(int folderId)
    {
        var folder = new Folder(id: 5, title: "Test folder");

        var completedTask = new UserTask(id: 1, title: "Completed task", description: "Test description");
        completedTask.Complete(DateTime.Now);
        folder.AddTask(completedTask);

        var incompleteTask = new UserTask(id: 2, title: "Incomplete task", description: "Test description");
        folder.AddTask(incompleteTask);

        return folder;
    }

    public IReadOnlyCollection<Folder> GetFolders()
    {
        return _db.Folders.Include(f => f.Tasks).ToList();
    }

    public void SaveNewTask(UserTask task, int folderId)
    {
        throw new System.NotImplementedException();
    }

    public void SaveNewFolder(Folder folder)
    {
        throw new System.NotImplementedException();
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
