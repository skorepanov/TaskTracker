namespace TaskTracker.Bll.Services;

public class TaskService(
    ITaskRepository taskRepository,
    IFolderRepository folderRepository,
    ITagRepository tagRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<Result<UserTaskVm>> GetTaskById(int taskId)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            return Result.Failure<UserTaskVm>(
                error: $"Задача не обнаружена (id = {taskId})");
        }

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return Result<UserTaskVm>.Success(taskVm);
    }

    public async Task<Result<IReadOnlyList<UserTaskVm>>> GetIncompletedTasks()
    {
        var tasks = await taskRepository.GetIncompletedTasks();

        var today = dateTimeProvider.UtcNow;
        var taskVms = tasks.Select(t => new UserTaskVm(t, today)).ToList();

        return Result<IReadOnlyList<UserTaskVm>>.Success(taskVms);
    }

    public async Task<Result<IReadOnlyList<UserTaskVm>>> GetCompletedTasks()
    {
        var tasks = await taskRepository.GetCompletedTasks();

        var today = dateTimeProvider.UtcNow;
        var taskVms = tasks.Select(t => new UserTaskVm(t, today)).ToList();

        return Result<IReadOnlyList<UserTaskVm>>.Success(taskVms);
    }

    public async Task<Result<IReadOnlyList<UserTaskVm>>> GetTasksInTrash()
    {
        var tasks = await taskRepository.GetTasksInTrash();

        var today = dateTimeProvider.UtcNow;
        var taskVms = tasks.Select(t => new UserTaskVm(t, today)).ToList();

        return Result<IReadOnlyList<UserTaskVm>>.Success(taskVms);
    }

    public async Task<Result<UserTaskVm>> CreateTask(UserTaskForCreationDto userTaskDto)
    {
        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var folder = await folderRepository.GetFolder(folderId.Value);

            if (folder is null)
            {
                return Result.Failure<UserTaskVm>(
                    error: $"Папка не обнаружена (id = {folderId.Value})");
            }
        }

        var newTaskResult = UserTask.CreateTask(userTaskDto, dateTimeProvider.UtcNow);

        if (!newTaskResult.IsOk)
        {
            return Result.Failure<UserTaskVm>(newTaskResult.Error ?? string.Empty);
        }

        await taskRepository.CreateTask(newTaskResult.Value);

        var newTaskVm = new UserTaskVm(newTaskResult.Value, dateTimeProvider.UtcNow);
        return Result<UserTaskVm>.Success(newTaskVm);
    }

    public async Task<Result<UserTaskVm>> UpdateTask(
        int taskId, UserTaskForUpdateDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            return Result.Failure<UserTaskVm>(
                error: $"Задача не обнаружена (id = {taskId})");
        }

        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var folder = await folderRepository.GetFolder(folderId.Value);

            if (folder is null)
            {
                return Result.Failure<UserTaskVm>(
                    error: $"Папка не обнаружена (id = {folderId.Value})");
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

                return Result.Failure<UserTaskVm>(
                    error: $"Теги не обнаружены (id = {nonExistentTagIdsAsString})");
            }
        }

        var updateResult = task.UpdateTask(
            userTaskDto, dateTimeProvider.UtcNow, tags);

        if (!updateResult.IsOk)
        {
            return Result.Failure<UserTaskVm>(updateResult.Error ?? string.Empty);
        }

        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return Result<UserTaskVm>.Success(taskVm);
    }

    public async Task<Result<UserTaskVm>> CompleteTask(
        int taskId, UserTaskForCompleteDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            return Result.Failure<UserTaskVm>(
                error: $"Задача не обнаружена (id = {taskId})");
        }

        var completedDateTime = userTaskDto.CompletedDateTime ?? dateTimeProvider.UtcNow;

        task.Complete(completedDateTime);
        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return Result<UserTaskVm>.Success(taskVm);
    }

    public async Task<Result<UserTaskVm>> IncompleteTask(
        int taskId, UserTaskForIncompleteDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            return Result.Failure<UserTaskVm>(
                error: $"Задача не обнаружена (id = {taskId})");
        }

        var modifiedDateTime = userTaskDto.ModifiedDateTime ?? dateTimeProvider.UtcNow;

        task.Incomplete(modifiedDateTime);
        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return Result<UserTaskVm>.Success(taskVm);
    }

    public async Task<Result<UserTaskVm>> MoveTaskToTrash(
        int taskId, UserTaskForMoveToTrashDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            return Result.Failure<UserTaskVm>(
                error: $"Задача не обнаружена (id = {taskId})");
        }

        var movedToTrashDateTime
            = userTaskDto.MovedToTrashDateTime ?? dateTimeProvider.UtcNow;

        var moveToTrashResult = task.MoveToTrash(movedToTrashDateTime);

        if (!moveToTrashResult.IsOk)
        {
            return Result.Failure<UserTaskVm>(
                moveToTrashResult.Error ?? string.Empty);
        }

        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return Result<UserTaskVm>.Success(taskVm);
    }

    public async Task<Result<UserTaskVm>> MoveTaskFromTrash(
        int taskId, UserTaskForMoveFromTrashDto userTaskDto)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            return Result.Failure<UserTaskVm>(
                error: $"Задача не обнаружена (id = {taskId})");
        }

        var folderId = userTaskDto.FolderId;

        if (folderId is not null)
        {
            var folder = await folderRepository.GetFolder(folderId.Value);

            if (folder is null)
            {
                return Result.Failure<UserTaskVm>(
                    error: $"Папка не обнаружена (id = {folderId.Value})");
            }
        }

        var modifiedDateTime
            = userTaskDto.ModifiedDateTime ?? dateTimeProvider.UtcNow;

        var moveFromTrashResult = task.MoveFromTrash(modifiedDateTime, folderId);

        if (!moveFromTrashResult.IsOk)
        {
            return Result.Failure<UserTaskVm>(
                moveFromTrashResult.Error ?? string.Empty);
        }

        await taskRepository.UpdateTask(task);

        var taskVm = new UserTaskVm(task, dateTimeProvider.UtcNow);
        return Result<UserTaskVm>.Success(taskVm);
    }

    public async Task<Result> DeleteTask(int taskId)
    {
        var task = await taskRepository.GetTask(taskId);

        if (task is null)
        {
            return Result.Failure<UserTaskVm>(
                error: $"Задача не обнаружена (id = {taskId})");
        }

        if (task.MovedToTrashDateTime is null)
        {
            return Result.Failure<UserTaskVm>(
                error: $"Невозможно удалить задачу не из корзины (id = {taskId})");
        }

        await taskRepository.DeleteTask(task);

        return Result.Success();
    }
}
