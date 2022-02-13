namespace TaskTracker.Bll;

public class UserTaskChangeData
{
    public int Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime? DueDate { get; init; }
    public int FolderId { get; init; }
}
