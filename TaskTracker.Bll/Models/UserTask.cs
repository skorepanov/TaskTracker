namespace TaskTracker.Bll.Models;

public class UserTask
{
    public int Id { get; }
    public string Title { get; private set; }
    public string? Description { get; private set; }

    public int? FolderId { get; private set; }
    public Folder? Folder { get; private set; }

    public DateTime? CompletedDateTime { get; private set; }

    public DateTime? DueDateTime { get; set; }

    public DateTime? MovedToTrashDateTime { get; private set; }
    public bool IsInTrash => MovedToTrashDateTime is not null;

    public List<Tag>? Tags { get; private set; }

    public DateTime CreatedDateTime { get; private set; }

    public DateTime? ModifiedDateTime { get; private set; }

    private UserTask(
        string title,
        int? folderId,
        DateTime? dueDateTime,
        DateTime createdDateTime)
    {
        Title = title;
        FolderId = folderId;
        DueDateTime = dueDateTime;
        CreatedDateTime = createdDateTime;
    }

    public static UserTask CreateTask(UserTaskForCreationDto userTaskDto, DateTime now)
    {
        var validationErrors = new List<string>();

        ValidateTitle(userTaskDto.Title, validationErrors);

        if (validationErrors.Count > 0)
        {
            var error = string.Join(separator: ", ",  validationErrors);
            throw new DomainException(error);
        }

        var normalizedTitle = userTaskDto.Title.Trim();
        var createdDateTime = userTaskDto.CreatedDateTime ?? now;

        var task = new UserTask(
            normalizedTitle,
            userTaskDto.FolderId,
            userTaskDto.DueDateTime,
            createdDateTime);

        return task;
    }

    public void UpdateTask(
        UserTaskForUpdateDto userTaskDto, DateTime now, IEnumerable<Tag> tags)
    {
        var validationErrors = new List<string>();

        ValidateTitle(userTaskDto.Title, validationErrors);

        if (validationErrors.Count > 0)
        {
            var error = string.Join(separator: ", ",  validationErrors);
            throw new DomainException(error);
        }

        Title = userTaskDto.Title.Trim();
        Description = userTaskDto.Description;
        FolderId = userTaskDto.FolderId;
        Tags = tags.ToList();
        DueDateTime = userTaskDto.DueDateTime;
        ModifiedDateTime = userTaskDto.ModifiedDateTime ?? now;
    }

    private static void ValidateTitle(string? title, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add(ErrorMessages.UserTaskTitleIsEmpty);
        }
    }

    public void Complete(DateTime completedDateTime)
    {
        CompletedDateTime = completedDateTime;
        ModifiedDateTime = completedDateTime;
    }

    public void Incomplete(DateTime modifiedDateTime)
    {
        CompletedDateTime = null;
        ModifiedDateTime = modifiedDateTime;
    }

    public int CalculateOverdueDays(DateTime today)
    {
        if (IsInTrash || DueDateTime is null || DueDateTime >= today)
        {
            return 0;
        }

        return (DueDateTime.Value - today).Duration().Days;
    }

    public Result MoveToTrash(DateTime movedToTrashDateTime)
    {
        if (IsInTrash)
        {
            var error = ErrorMessages.UserTaskIsInTrashAlready(Id);
            return Result.Failure(error);
        }

        MovedToTrashDateTime = movedToTrashDateTime;
        ModifiedDateTime = movedToTrashDateTime;
        FolderId = null;

        return Result.Success();
    }

    public Result MoveFromTrash(DateTime modifiedDateTime, int? folderId)
    {
        if (!IsInTrash)
        {
            var error = ErrorMessages.UserTaskIsNotInTrash(Id);
            return Result.Failure(error);
        }

        MovedToTrashDateTime = null;
        ModifiedDateTime = modifiedDateTime;
        FolderId = folderId;

        return Result.Success();
    }
}
