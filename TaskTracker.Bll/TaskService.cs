using System.Threading.Tasks;

namespace TaskTracker.Bll;

public class TaskService(ITaskRepository _taskRepository,
                         IFolderRepository _folderRepository)
{
    public async Task<UserTask> GetTaskById(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        return task;
    }

    public async Task<IReadOnlyList<UserTask>> GetIncompleteTasks(int folderId)
    {
        var folder = await _folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        return folder.IncompleteTasks;
    }

    public async Task<IReadOnlyList<UserTask>> GetCompletedTasks(int folderId)
    {
        var folder = await _folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        return folder.CompletedTasks;
    }

    public async Task<IReadOnlyList<UserTask>> GetTodayTasks()
    {
        var today = DateTime.Now;
        var tasks = await _taskRepository.GetNonDeletedTasks();

        var todayTasks = tasks.Where(t => t.IsTodayTask(today)).ToList();

        return todayTasks;
    }

    public async Task<IReadOnlyList<UserTask>> GetTasksInTrash()
    {
        var tasks = await _taskRepository.GetTasksInTrash();
        return tasks;
    }

    public async Task<UserTask> CreateTask(UserTaskForCreationDto userTaskDto)
    {
        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var folder = await _folderRepository.GetFolder(folderId.Value);

            if (folder is null)
            {
                throw new DomainEntityNotFoundException(
                    domainEntityType: typeof(Folder),
                    message: $"Папка не обнаружена (id = {folderId.Value})");
            }
        }

        var newTask = UserTask.CreateTask(userTaskDto);
        await _taskRepository.CreateTask(newTask, folderId);
        return newTask;
    }

    public async Task<UserTask> UpdateTask(int taskId, UserTaskForUpdateDto userTaskDto)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var folder = await _folderRepository.GetFolder(folderId.Value);

            if (folder is null)
            {
                throw new DomainEntityNotFoundException(
                    domainEntityType: typeof(Folder),
                    message: $"Папка не обнаружена (id = {folderId.Value})");
            }
        }

        task.UpdateTask(userTaskDto);
        await _taskRepository.UpdateTaskWithFolder(task, folderId);

        return task;
    }

    public async Task CompleteTask(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        task.Complete(DateTime.Now);
        await _taskRepository.UpdateTask(task);
    }

    public async Task IncompleteTask(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        task.Incomplete();
        await _taskRepository.UpdateTask(task);
    }

    public async Task DeleteTask(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        task.Delete(new DateTime());
        await _taskRepository.UpdateTask(task);
    }

    public async Task DeleteTaskPermanently(int taskId)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        if (task.DeletionDate is null)
        {
            throw new CannotDeleteDomainEntityException(
                domainEntityType: typeof(UserTask),
                domainEntityId: task.Id,
                message: "Невозможно удалить задачу не из корзины");
        }

        await _taskRepository.DeleteTaskPermanently(task);
    }

    public async Task<Folder> GetFolderById(int folderId)
    {
        var folder = await _folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        return folder;
    }

    public async Task<IReadOnlyList<Folder>> GetFolders()
    {
        var folders = await _folderRepository.GetFolders();
        return folders;
    }

    public async Task<Folder> CreateFolder(FolderForCreationDto folderDto)
    {
        var newFolder = Folder.CreateFolder(folderDto);
        await _folderRepository.CreateFolder(newFolder);
        return newFolder;
    }

    public async Task<Folder> UpdateFolder(int folderId, FolderForUpdateDto folderDto)
    {
        var folder = await _folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        folder.UpdateFolder(folderDto);
        await _folderRepository.UpdateFolder(folder);

        return folder;
    }

    public async Task DeleteFolder(int folderId)
    {
        var folder = await _folderRepository.GetFolder(folderId);

        if (folder is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Folder),
                message: $"Папка не обнаружена (id = {folderId})");
        }

        await _folderRepository.DeleteFolder(folder);
    }
}
