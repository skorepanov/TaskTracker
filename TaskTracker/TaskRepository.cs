namespace TaskTracker;

public class TaskRepository : ITaskRepository
{
    public UserTask GetTask(int taskId)
    {
        throw new System.NotImplementedException();
    }

    public Folder GetFolder(int folderId)
    {
        throw new System.NotImplementedException();
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
