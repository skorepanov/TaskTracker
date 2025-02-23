namespace TaskTracker.Bll.Models;

public class UserTask
{
    public int Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    public int? FolderId { get; private set; }
    public Folder? Folder { get; private set; }

    public DateTime? CompletionDate { get; private set; }
    public bool IsCompleted => CompletionDate is not null;

    public DateTime? DueDate { get; set; }

    public DateTime? DeletionDate { get; private set; }
    public bool IsDeleted => DeletionDate is not null;

    private UserTask(string title, string description, int? folderId, DateTime? dueDate)
    {
        Title = title;
        Description = description;
        FolderId = folderId;
        DueDate = dueDate;
    }

    public static UserTask CreateTask(UserTaskForCreationDto userTaskDto)
    {
        var normalizedTitle = userTaskDto.Title.Trim();
        var normalizedDescription = userTaskDto.Description.Trim();

        return new UserTask(
            normalizedTitle, normalizedDescription,
            userTaskDto.FolderId, userTaskDto.DueDate);
    }

    public void UpdateTask(UserTaskForUpdateDto userTaskDto)
    {
        Title = userTaskDto.Title.Trim();
        Description = userTaskDto.Description.Trim();
        FolderId = userTaskDto.FolderId;
        DueDate = userTaskDto.DueDate;
    }

    public void Complete(DateTime completionDate)
    {
        CompletionDate = completionDate;
    }

    public void Incomplete()
    {
        CompletionDate = null;
    }

    public int CalculateOverdueDays(DateTime today)
    {
        if (IsDeleted || DueDate is null || DueDate >= today)
        {
            return 0;
        }

        return (DueDate.Value - today).Duration().Days;
    }

    public bool IsTodayTask(DateTime today)
    {
        if (IsDeleted)
        {
            return false;
        }

        return CompletionDate?.Date == today.Date
           || !IsCompleted && DueDate?.Date <= today.Date;
    }

    public void Delete(DateTime deletionDate)
    {
        DeletionDate = deletionDate;
    }
}
