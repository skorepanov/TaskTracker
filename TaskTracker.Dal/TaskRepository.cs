using TaskTracker.Bll;

namespace TaskTracker.Dal;

public class TaskRepository : ITaskRepository
{
    public UserTask GetTask(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public IReadOnlyCollection<UserTask> GetNonDeletedTasks()
    {
        var completedTask = new UserTask(title: "Completed task", description: "Description");
        completedTask.Complete(DateTime.Now);

        var incompleteTask = new UserTask(title: "Overdue task", description: "Description");
        incompleteTask.DueDate = DateTime.Now.AddDays(-1);

        var tasks = new List<UserTask> { completedTask, incompleteTask };
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
