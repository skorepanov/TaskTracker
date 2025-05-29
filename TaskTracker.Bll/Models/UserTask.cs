namespace TaskTracker.Bll.Models;

public class UserTask
{
    public int Id { get; }
    public string Title { get; private set; }
    public string? Description { get; private set; }

    public int? FolderId { get; private set; }
    public Folder? Folder { get; private set; }

    public DateTime? CompletedDateTime { get; private set; }
    public bool IsCompleted => CompletedDateTime is not null;

    public DateTime? DueDateTime { get; set; }

    public DateTime? MovedToTrashDateTime { get; private set; }
    public bool IsInTrash => MovedToTrashDateTime is not null;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime? ModifiedDateTime { get; private set; }

    private UserTask(
        string title,
        string? description,
        int? folderId,
        DateTime? dueDateTime,
        DateTime createdDateTime)
    {
        Title = title;
        Description = description;
        FolderId = folderId;
        DueDateTime = dueDateTime;
        CreatedDateTime = createdDateTime;
    }

    public static UserTask CreateTask(UserTaskForCreationDto userTaskDto, DateTime now)
    {
        var normalizedTitle = userTaskDto.Title.Trim();
        var normalizedDescription = userTaskDto.Description?.Trim();
        var createdDateTime = userTaskDto.CreatedDateTime ?? now;

        return new UserTask(
            normalizedTitle,
            normalizedDescription,
            userTaskDto.FolderId,
            userTaskDto.DueDateTime,
            createdDateTime);
    }

    public void UpdateTask(UserTaskForUpdateDto userTaskDto, DateTime now)
    {
        Title = userTaskDto.Title.Trim();
        Description = userTaskDto.Description?.Trim();
        FolderId = userTaskDto.FolderId;
        DueDateTime = userTaskDto.DueDateTime;
        ModifiedDateTime = userTaskDto.ModifiedDateTime ?? now;
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

    public void MoveToTrash(DateTime movedToTrashDateTime)
    {
        if (IsInTrash)
        {
            return;
        }

        MovedToTrashDateTime = movedToTrashDateTime;
        ModifiedDateTime = movedToTrashDateTime;
        FolderId = null;
    }

    public void MoveFromTrash(DateTime modifiedDateTime)
    {
        if (!IsInTrash)
        {
            return;
        }

        MovedToTrashDateTime = null;
        ModifiedDateTime = modifiedDateTime;
    }
}
