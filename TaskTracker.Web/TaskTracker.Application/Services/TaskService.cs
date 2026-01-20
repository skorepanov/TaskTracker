namespace TaskTracker.Application.Services;

public class TaskService(
    ITaskRepository taskRepository,
    IFolderRepository folderRepository,
    ITagRepository tagRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<UserTaskVm> GetTaskById(int taskId)
    {
        var task = await taskRepository.GetTask(taskId);
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
            var isFolderExists = await folderRepository.IsFolderExists(folderId.Value);

            if (!isFolderExists)
            {
                var error = ErrorMessages.FolderNotFound(folderId.Value);
                throw new DomainException(error);
            }
        }

        var newTask = UserTask.CreateTask(userTaskDto, dateTimeProvider.UtcNow);

        await taskRepository.CreateTask(newTask);

        var newTaskVm = new UserTaskVm(newTask, dateTimeProvider.UtcNow);
        return newTaskVm;
    }

    public async Task<UserTaskVm> UpdateTask(
        int taskId, UserTaskForUpdateDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);
        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var isFolderExists = await folderRepository.IsFolderExists(folderId.Value);

            if (!isFolderExists)
            {
                var error = ErrorMessages.FolderNotFound(folderId.Value);
                throw new DomainException(error);
            }
        }

        var tagIds = userTaskDto.TagIds?.Distinct().ToList() ?? [];

        var tags = tagIds.Count > 0
            ? await tagRepository.GetTags(tagIds)
            : [];

        task.UpdateTask(userTaskDto, dateTimeProvider.UtcNow, tags);

        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task<UserTaskVm> CompleteTask(
        int taskId, UserTaskForCompleteDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

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

        var movedToTrashDateTime
            = userTaskDto.MovedToTrashDateTime ?? dateTimeProvider.UtcNow;
        task.MoveToTrash(movedToTrashDateTime);

        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task<UserTaskVm> MoveTaskFromTrash(
        int taskId, UserTaskForMoveFromTrashDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);
        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var isFolderExists = await folderRepository.IsFolderExists(folderId.Value);

            if (!isFolderExists)
            {
                var error = ErrorMessages.FolderNotFound(folderId.Value);
                throw new DomainException(error);
            }
        }

        var modifiedDateTime
            = userTaskDto.ModifiedDateTime ?? dateTimeProvider.UtcNow;
        task.MoveFromTrash(modifiedDateTime, folderId);

        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return taskVm;
    }

    public async Task<bool> DeleteTask(int taskId)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task.MovedToTrashDateTime is null)
        {
            var error = ErrorMessages.CantDeleteUserTaskThatIsNotInTrash(taskId);
            throw new DomainException(error);
        }

        await taskRepository.DeleteTask(task);

        return true;
    }
}
