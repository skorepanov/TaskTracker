namespace TaskTracker.Bll.Models;

public class Folder
{
    public int Id { get; }
    public string Title { get; private set; }

    private readonly List<UserTask> _tasks;
    public IReadOnlyList<UserTask> Tasks => _tasks;

    public IReadOnlyList<UserTask> CompletedTasks
        => _tasks
            .Where(t => t is { IsInTrash: false, IsCompleted: true })
            .ToList();

    public IReadOnlyList<UserTask> IncompleteTasks
        => _tasks
            .Where(t => t is { IsInTrash: false, IsCompleted: false })
            .ToList();

    public int IncompleteTaskCount => IncompleteTasks.Count;

    public DateTime CreatedDateTime { get; private set; }

    public DateTime? ModifiedDateTime { get; private set; }

    private Folder(string title, DateTime createdDateTime)
    {
        Title = title;
        _tasks = [];
        CreatedDateTime = createdDateTime;
    }

    public static Folder CreateFolder(
        FolderForCreationDto folderDto,
        DateTime now)
    {
        var normalizedTitle = folderDto.Title.Trim();
        var createdDateTime = folderDto.CreatedDateTime ?? now;

        return new Folder(normalizedTitle, createdDateTime);
    }

    public void UpdateFolder(
        FolderForUpdateDto folderDto,
        DateTime now)
    {
        Title = folderDto.Title.Trim();
        ModifiedDateTime = folderDto.ModifiedDateTime ?? now;
    }

    public void AddTask(UserTask task)
    {
        if (!_tasks.Contains(task))
        {
            _tasks.Add(task);
        }
    }
}
