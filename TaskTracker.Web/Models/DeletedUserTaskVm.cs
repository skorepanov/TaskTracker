namespace TaskTracker.Web.Models;

public class DeletedUserTaskVm
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; }
    public DateTime? CompletionDate { get; }
    public bool IsCompleted { get; }
    public DateTime? DueDate { get; }
    public DateTime DeletionDate { get; }

    public DeletedUserTaskVm(UserTask task)
    {
        this.Id = task.Id;
        this.Title = task.Title;
        this.Description = task.Description;
        this.CompletionDate = task.CompletionDate;
        this.IsCompleted = task.IsCompleted;
        this.DueDate = task.DueDate;
        this.DeletionDate = task.DeletionDate!.Value;
    }

    public static IReadOnlyCollection<DeletedUserTaskVm> CreateCollectionFrom(
        IEnumerable<UserTask> tasks)
    {
        return tasks.Select(t => new DeletedUserTaskVm(t)).ToList();
    }
}
