namespace TaskTracker;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        this._taskRepository = taskRepository;
    }

    public UserTask CreateTask(string title, string description, int folderId)
    {
        var newTask = new UserTask(title, description);
        var folder = _taskRepository.GetFolder(folderId);

        if (folder is null)
        {
            return null;
        }

        _taskRepository.SaveNewTask(newTask, folderId);
        return newTask;
    }

    public Folder CreateFolder(string title)
    {
        var newFolder = new Folder(title);
        _taskRepository.SaveNewFolder(newFolder);
        return newFolder;
    }
}
