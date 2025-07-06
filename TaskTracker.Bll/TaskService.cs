namespace TaskTracker.Bll;

public class TaskService(
    ITaskRepository taskRepository,
    IFolderRepository folderRepository,
    ITagRepository tagRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<UserTaskVm> GetTaskById(int taskId)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task<IReadOnlyList<UserTaskVm>> GetIncompletedTasks()
    {
        var tasks = await taskRepository.GetIncompletedTasks();

        var today = dateTimeProvider.UtcNow;
        var taskVms = tasks.Select(t => new UserTaskVm(t, today)).ToList();

        return taskVms;
    }

    public async Task<IReadOnlyList<UserTaskVm>> GetCompletedTasks()
    {
        var tasks = await taskRepository.GetCompletedTasks();

        var today = dateTimeProvider.UtcNow;
        var taskVms = tasks.Select(t => new UserTaskVm(t, today)).ToList();

        return taskVms;
    }

    public async Task<IReadOnlyList<UserTaskVm>> GetTasksInTrash()
    {
        var tasks = await taskRepository.GetTasksInTrash();

        var today = dateTimeProvider.UtcNow;
        var taskVms = tasks.Select(t => new UserTaskVm(t, today)).ToList();

        return taskVms;
    }

    public async Task<UserTaskVm> CreateTask(UserTaskForCreationDto userTaskDto)
    {
        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var folder = await folderRepository.GetFolder(folderId.Value);

            if (folder is null)
            {
                throw new DomainEntityNotFoundException(
                    domainEntityType: typeof(Folder),
                    message: $"Папка не обнаружена (id = {folderId.Value})");
            }
        }

        var newTask = UserTask.CreateTask(userTaskDto, dateTimeProvider.UtcNow);
        await taskRepository.CreateTask(newTask);

        var newTaskVm = new UserTaskVm(newTask, dateTimeProvider.UtcNow);
        return newTaskVm;
    }

    public async Task<UserTaskVm> UpdateTask(int taskId, UserTaskForUpdateDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var folder = await folderRepository.GetFolder(folderId.Value);

            if (folder is null)
            {
                throw new DomainEntityNotFoundException(
                    domainEntityType: typeof(Folder),
                    message: $"Папка не обнаружена (id = {folderId.Value})");
            }
        }

        var tagIds = userTaskDto.TagIds ?? [];
        IReadOnlyList<Tag> tags = new List<Tag>();

        if (tagIds.Count > 0)
        {
            tags = await tagRepository.GetTags(tagIds);

            var existentTagIds = tags.Select(t => t.Id).ToList();
            var nonExistentTagIds = tagIds.Except(existentTagIds).ToList();

            if (nonExistentTagIds.Count > 0)
            {
                var nonExistentTagIdsAsString = string.Join(", ", nonExistentTagIds);

                throw new DomainEntityNotFoundException(
                    domainEntityType: typeof(Tag),
                    message: $"Теги не обнаружены (id = {nonExistentTagIdsAsString})");
            }
        }

        task.UpdateTask(userTaskDto, dateTimeProvider.UtcNow, tags);
        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task<UserTaskVm> CompleteTask(
        int taskId, UserTaskForCompleteDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var completedDateTime = userTaskDto.CompletedDateTime ?? dateTimeProvider.UtcNow;

        task.Complete(completedDateTime);
        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task<UserTaskVm> IncompleteTask(
        int taskId, UserTaskForIncompleteDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var modifiedDateTime = userTaskDto.ModifiedDateTime ?? dateTimeProvider.UtcNow;

        task.Incomplete(modifiedDateTime);
        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task<UserTaskVm> MoveTaskToTrash(
        int taskId, UserTaskForMoveToTrashDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var movedToTrashDateTime = userTaskDto.MovedToTrashDateTime ?? dateTimeProvider.UtcNow;

        task.MoveToTrash(movedToTrashDateTime);
        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task<UserTaskVm> MoveTaskFromTrash(
        int taskId, UserTaskForMoveFromTrashDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(UserTask),
                message: $"Задача не обнаружена (id = {taskId})");
        }

        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var folder = await folderRepository.GetFolder(folderId.Value);

            if (folder is null)
            {
                throw new DomainEntityNotFoundException(
                    domainEntityType: typeof(Folder),
                    message: $"Папка не обнаружена (id = {folderId.Value})");
            }
        }

        var modifiedDateTime = userTaskDto.ModifiedDateTime ?? dateTimeProvider.UtcNow;

        task.MoveFromTrash(modifiedDateTime, folderId);
        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task DeleteTask(int taskId)
    {
        var task = await taskRepository.GetTask(taskId);

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

        await taskRepository.DeleteTask(task);
    }
}
