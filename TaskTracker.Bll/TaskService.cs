namespace TaskTracker.Bll;

public class TaskService(
    ITaskRepository _taskRepository,
    IFolderRepository _folderRepository,
    ITagRepository _tagRepository,
    IDateTimeProvider _dateTimeProvider)
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

        var newTask = UserTask.CreateTask(userTaskDto, _dateTimeProvider.UtcNow);
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

        var tagIds = userTaskDto.TagIds ?? [];
        IReadOnlyList<Tag> tags = new List<Tag>();

        if (tagIds.Count > 0)
        {
            tags = await _tagRepository.GetTags(tagIds);

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

        task.UpdateTask(userTaskDto, _dateTimeProvider.UtcNow, tags);
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

        var completedDateTime = userTaskDto.CompletedDateTime ?? _dateTimeProvider.UtcNow;

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

        var modifiedDateTime = userTaskDto.ModifiedDateTime ?? _dateTimeProvider.UtcNow;

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

        var movedToTrashDateTime = userTaskDto.MovedToTrashDateTime ?? _dateTimeProvider.UtcNow;

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

        var modifiedDateTime = userTaskDto.ModifiedDateTime ?? _dateTimeProvider.UtcNow;

        task.MoveFromTrash(modifiedDateTime, folderId);
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
        var newTag = Tag.CreateTag(tagDto, _dateTimeProvider.UtcNow);
        await _tagRepository.CreateTag(newTag);

        return newTag;
    }

    public async Task<Tag> UpdateTag(int tagId, TagForUpdateDto tagDto)
    {
        var tag = await _tagRepository.GetTag(tagId);

        if (tag is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Tag),
                message: $"Тег не обнаружен (id = {tagId})");
        }

        tag.UpdateTag(tagDto, _dateTimeProvider.UtcNow);
        await _tagRepository.UpdateTag(tag);

        return tag;
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
