namespace TaskTracker.Bll.Models;

public class UserTask
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }

    public DateTime? CompletionDate { get; private set; }
    public bool IsCompleted => CompletionDate is not null;

    public DateTime? DueDate { get; set; }

    public DateTime? DeletionDate { get; private set; }
    public bool IsDeleted => DeletionDate is not null;

    private UserTask(string title, string description, DateTime? dueDate)
    {
        this.Title = title;
        this.Description = description;
        this.DueDate = dueDate;
    }

    public static UserTask CreateTask(UserTaskForCreationDto userTaskDto)
    {
        return new UserTask(userTaskDto.Title, userTaskDto.Description, userTaskDto.DueDate);
    }

    public void Complete(DateTime completionDate)
    {
        this.CompletionDate = completionDate;
    }

    public void Incomplete()
    {
        this.CompletionDate = null;
    }

    public int CalculateOverdueDays(DateTime today)
    {
        if (this.IsDeleted || this.DueDate is null || this.DueDate >= today)
        {
            return 0;
        }

        return (this.DueDate.Value - today).Duration().Days;
    }

    public bool IsTodayTask(DateTime today)
    {
        if (this.IsDeleted)
        {
            return false;
        }

        return this.CompletionDate?.Date == today.Date
           || !this.IsCompleted && this.DueDate?.Date <= today.Date;
    }

    public void Delete(DateTime deletionDate)
    {
        this.DeletionDate = deletionDate;
    }
}
