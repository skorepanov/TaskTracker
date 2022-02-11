using System.Threading.Tasks;

namespace TaskTracker.Bll;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        this._taskRepository = taskRepository;
    }

    public async Task<UserTask> CreateTask(UserTaskChangeData changeData)
    {
        var folder = await _taskRepository.GetFolder(changeData.FolderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(typeof(Folder),
                                                    message: "Папка не обнаружена");
        }

        var newTask = UserTask.CreateTask(changeData);
        await _taskRepository.SaveNewTask(newTask, folder.Id);
        return newTask;
    }

    public async Task CompleteTask(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        task.Complete(new DateTime());
        await _taskRepository.UpdateTask(task);
    }

    public async Task IncompleteTask(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        task.Incomplete();
        await _taskRepository.UpdateTask(task);
    }

    public async Task DeleteTask(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        task.Delete(new DateTime());
        await _taskRepository.UpdateTask(task);
    }

    public async Task<IReadOnlyCollection<UserTask>> GetIncompleteTasks(int folderId)
    {
        // TODO check folder for null
        var folder = await _taskRepository.GetFolder(folderId);
        return folder.IncompleteTasks;
    }

    public async Task<IReadOnlyCollection<UserTask>> GetCompletedTasks(int folderId)
    {
        // TODO check folder for null
        var folder = await _taskRepository.GetFolder(folderId);
        return folder.CompletedTasks;
    }

    public async Task<IReadOnlyCollection<UserTask>> GetTodayTasks()
    {
        var today = DateTime.Now;
        var tasks = await _taskRepository.GetNonDeletedTasks();

        var todayTasks = tasks.Where(t => t.IsTodayTask(today)).ToList();

        return todayTasks;
    }

    public async Task<IReadOnlyCollection<UserTask>> GetDeletedTasks()
    {
        var tasks = await _taskRepository.GetDeletedTasks();
        return tasks;
    }

    public async Task<IReadOnlyCollection<Folder>> GetFolders()
    {
        var folders = await _taskRepository.GetFolders();
        return folders;
    }

    public async Task<Folder> CreateFolder(FolderChangeData changeData)
    {
        var newFolder = Folder.CreateFolder(changeData);
        await _taskRepository.SaveNewFolder(newFolder);
        return newFolder;
    }

    public async Task MoveTaskToOtherFolder(int taskId, int destinationFolderId)
    {
        var task = await _taskRepository.GetTask(taskId);
        if (task is null)
        {
            throw new DomainEntityNotFoundException(domainEntityType: typeof(UserTask),
                                                    message: "Задача не обнаружена");
        }

        var folder = await _taskRepository.GetFolder(destinationFolderId);
        if (folder is null)
        {
            throw new DomainEntityNotFoundException(typeof(Folder),
                                                    message: "Папка не обнаружена");
        }

        await _taskRepository.UpdateTaskFolder(taskId, destinationFolderId);
    }
}
