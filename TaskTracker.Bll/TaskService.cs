using System.Threading.Tasks;

namespace TaskTracker.Bll;

public class TaskService(
    ITaskRepository _taskRepository,
    IFolderRepository _folderRepository,
    ITagRepository _tagRepository)
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

    public async Task<IReadOnlyList<UserTask>> GetIncompletedTasks()
    {
        var tasks = await _taskRepository.GetIncompletedTasks();
        return tasks;
    }

    public async Task<IReadOnlyList<UserTask>> GetCompletedTasks()
    {
        var tasks = await _taskRepository.GetCompletedTasks();
        return tasks;
    }

    public async Task<IReadOnlyList<UserTask>> GetTodayTasks()
    {
        var today = DateTime.UtcNow;
        var tasks = await _taskRepository.GetNonMovedToTrashTasks();

        var todayTasks = tasks.Where(t => t.IsTodayTask(today)).ToList();

        return todayTasks;
    }

    public async Task<IReadOnlyList<UserTask>> GetTasksInInbox()
    {
        var tasks = await _taskRepository.GetTasksInInbox();
        return tasks;
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

        var now = DateTime.UtcNow;
        var newTask = UserTask.CreateTask(userTaskDto, now);
        await _taskRepository.CreateTask(newTask);

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

        var now = DateTime.UtcNow;
        task.UpdateTask(userTaskDto, now);
        await _taskRepository.UpdateTask(task);

        return task;
    }

    public async Task<UserTask> CompleteTask(
        int taskId, UserTaskForCompleteDto userTaskDto)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var completedDateTime = userTaskDto.CompletedDateTime ?? DateTime.UtcNow;

        task.Complete(completedDateTime);
        await _taskRepository.UpdateTask(task);

        return task;
    }

    public async Task<UserTask> IncompleteTask(
        int taskId, UserTaskForIncompleteDto userTaskDto)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var modifiedDateTime = userTaskDto.ModifiedDateTime ?? DateTime.UtcNow;

        task.Incomplete(modifiedDateTime);
        await _taskRepository.UpdateTask(task);

        return task;
    }

    public async Task<UserTask> MoveTaskToTrash(
        int taskId, UserTaskForMoveToTrashDto userTaskDto)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var movedToTrashDateTime = userTaskDto.MovedToTrashDateTime ?? DateTime.UtcNow;

        task.MoveToTrash(movedToTrashDateTime);
        await _taskRepository.UpdateTask(task);

        return task;
    }

    public async Task<UserTask> MoveTaskFromTrash(
        int taskId, UserTaskForMoveFromTrashDto userTaskDto)
    {
        var task = await _taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var modifiedDateTime = userTaskDto.ModifiedDateTime ?? DateTime.UtcNow;

        task.MoveFromTrash(modifiedDateTime);
        await _taskRepository.UpdateTask(task);

        return task;
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

        if (task.MovedToTrashDateTime is null)
        {
            throw new CannotDeleteDomainEntityException(
                domainEntityType: typeof(UserTask),
                domainEntityId: task.Id,
                message: $"Невозможно удалить задачу не из корзины (id = {taskId})");
        }

        await _taskRepository.DeleteTask(task);
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
        var now = DateTime.UtcNow;
        var newFolder = Folder.CreateFolder(folderDto, now);
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

        var now =  DateTime.UtcNow;
        folder.UpdateFolder(folderDto, now);
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

    public async Task<Tag> GetTagById(int tagId)
    {
        var tag = await _tagRepository.GetTag(tagId);

        if (tag is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Tag),
                message: $"Тег не обнаружен (id = {tagId})");
        }

        return tag;
    }

    public async Task<IReadOnlyList<Tag>> GetTags()
    {
        var tags = await _tagRepository.GetTags();
        return tags;
    }

    public async Task<Tag> CreateTag(TagForCreationDto tagDto)
    {
        var now = DateTime.UtcNow;
        var newTag = Tag.CreateTag(tagDto, now);
        await _tagRepository.CreateTag(newTag);

        return newTag;
    }

    public async Task DeleteTag(int tagId)
    {
        var tag = await _tagRepository.GetTag(tagId);

        if (tag is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Tag),
                message: $"Тег не обнаружен (id = {tagId})");
        }

        await _tagRepository.DeleteTag(tag);
    }
}
