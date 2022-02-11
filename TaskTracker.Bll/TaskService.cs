namespace TaskTracker.Bll;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        this._taskRepository = taskRepository;
    }

    public UserTask CreateTask(UserTaskChangeData changeData)
    {
        var folder = _taskRepository.GetFolder(changeData.FolderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(typeof(Folder),
                                                    message: "Папка не обнаружена");
        }

        var newTask = UserTask.CreateTask(changeData);
        _taskRepository.SaveNewTask(newTask, folder.Id);
        return newTask;
    }

    public void CompleteTask(int taskId)
    {
        var task = _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        task.Complete(new DateTime());
        _taskRepository.UpdateTask(task);
    }

    public void IncompleteTask(int taskId)
    {
        var task = _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        task.Incomplete();
        _taskRepository.UpdateTask(task);
    }

    public void DeleteTask(int taskId)
    {
        var task = _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        task.Delete(new DateTime());
        _taskRepository.UpdateTask(task);
    }

    public IReadOnlyCollection<UserTask> GetIncompleteTasks(int folderId)
    {
        // TODO check folder for null
        var folder = _taskRepository.GetFolder(folderId);
        return folder.IncompleteTasks;
    }

    public IReadOnlyCollection<UserTask> GetCompletedTasks(int folderId)
    {
        // TODO check folder for null
        var folder = _taskRepository.GetFolder(folderId);
        return folder.CompletedTasks;
    }

    public IReadOnlyCollection<UserTask> GetTodayTasks()
    {
        var today = DateTime.Now;
        var tasks = _taskRepository.GetNonDeletedTasks();

        var todayTasks = tasks.Where(t => t.IsTodayTask(today)).ToList();

        return todayTasks;
    }

    public IReadOnlyCollection<UserTask> GetDeletedTasks()
    {
        var tasks = _taskRepository.GetDeletedTasks();
        return tasks;
    }

    public IReadOnlyCollection<Folder> GetFolders()
    {
        var folders = _taskRepository.GetFolders();
        return folders;
    }

    public Folder CreateFolder(FolderChangeData changeData)
    {
        var newFolder = Folder.CreateFolder(changeData);
        _taskRepository.SaveNewFolder(newFolder);
        return newFolder;
    }

    public void MoveTaskToOtherFolder(int taskId, int destinationFolderId)
    {
        if (_taskRepository.GetTask(taskId) is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        if (_taskRepository.GetFolder(destinationFolderId) is null)
        {
            throw new DomainEntityNotFoundException(typeof(Folder),
                                                    message: "Папка не обнаружена");
        }

        _taskRepository.UpdateTaskFolder(taskId, destinationFolderId);
    }
}
