namespace TaskTracker.Bll.Models;

public class UserTask
{
    public int Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    public int? FolderId { get; private set; }
    public Folder? Folder { get; private set; }

    public DateTime? CompletedDateTime { get; private set; }
    public bool IsCompleted => CompletedDateTime is not null;

    public DateTime? DueDateTime { get; set; }

    public DateTime? DeletionDate { get; private set; }
    public bool IsDeleted => DeletionDate is not null;

    private UserTask(string title, string description, int? folderId, DateTime? dueDateTime)
    {
        Title = title;
        Description = description;
        FolderId = folderId;
        DueDateTime = dueDateTime;
    }

    public static UserTask CreateTask(UserTaskForCreationDto userTaskDto)
    {
        var normalizedTitle = userTaskDto.Title.Trim();
        var normalizedDescription = userTaskDto.Description.Trim();

        return new UserTask(
            normalizedTitle, normalizedDescription,
            userTaskDto.FolderId, userTaskDto.DueDateTime);
    }

    public void UpdateTask(UserTaskForUpdateDto userTaskDto)
    {
        Title = userTaskDto.Title.Trim();
        Description = userTaskDto.Description.Trim();
        FolderId = userTaskDto.FolderId;
        DueDateTime = userTaskDto.DueDateTime;
    }

    public void Complete(DateTime completedDateTime)
    {
        CompletedDateTime = completedDateTime;
    }

    public void Incomplete()
    {
        CompletedDateTime = null;
    }

    public int CalculateOverdueDays(DateTime today)
    {
        if (IsDeleted || DueDateTime is null || DueDateTime >= today)
        {
            return 0;
        }

        return (DueDateTime.Value - today).Duration().Days;
    }

    public bool IsTodayTask(DateTime today)
    {
        if (IsDeleted)
        {
            return false;
        }

        return CompletedDateTime?.Date == today.Date
           || !IsCompleted && DueDateTime?.Date <= today.Date;
    }

    public void Delete(DateTime deletionDate)
    {
        DeletionDate = deletionDate;
    }
}
