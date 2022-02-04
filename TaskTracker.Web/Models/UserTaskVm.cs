using TaskTracker.Bll;

namespace TaskTracker.Web;

// TODO create several view models for completed and incomplete tasks
public class UserTaskVm
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime? CompletionDate { get; }
    public bool IsCompleted { get; }
    public DateTime? DueDate { get; }
    public int OverdueDaysCount { get; }
    public DateTime? DeletionDate { get; }
    public bool IsDeleted { get; }

    public UserTaskVm(UserTask task, DateTime today)
    {
        this.Id = task.Id;
        this.Title = task.Title;
        this.Description = task.Description;
        this.CompletionDate = task.CompletionDate;
        this.IsCompleted = task.IsCompleted;
        this.DueDate = task.DueDate;
        this.OverdueDaysCount = task.CalculateOverdueDays(today);
        this.DeletionDate = task.DeletionDate;
        this.IsDeleted = task.IsDeleted;
    }

    public static IReadOnlyCollection<UserTaskVm> CreateCollectionFrom(
        IEnumerable<UserTask> tasks, DateTime today)
    {
        return tasks.Select(t => new UserTaskVm(t, today)).ToList();
    }
}
