namespace TaskTracker.Bll.Models;

public class Folder
{
    public int Id { get; }
    public string Title { get; private set; }

    private readonly List<UserTask> _tasks;
    public IReadOnlyList<UserTask> Tasks => _tasks;

    public IReadOnlyList<UserTask> CompletedTasks
        => _tasks.Where(t => !t.IsDeleted && t.IsCompleted).ToList();

    public IReadOnlyList<UserTask> IncompleteTasks
        => _tasks.Where(t => !t.IsCompleted && !t.IsDeleted).ToList();

    public int IncompleteTaskCount => IncompleteTasks.Count;

    private Folder(string title)
    {
        Title = title;
        _tasks = [];
    }

    public static Folder CreateFolder(FolderForCreationDto folderDto)
    {
        var normalizedTitle = folderDto.Title.Trim();
        return new Folder(normalizedTitle);
    }

    public void UpdateFolder(FolderForUpdateDto folderDto)
    {
        Title = folderDto.Title.Trim();
    }

    public void AddTask(UserTask task)
    {
        if (!_tasks.Contains(task))
        {
            _tasks.Add(task);
        }
    }
}
