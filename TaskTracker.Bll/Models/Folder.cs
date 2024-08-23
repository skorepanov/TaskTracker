namespace TaskTracker.Bll.Models;

public class Folder
{
    public int Id { get; private set; }
    public string Title { get; private set; }

    private readonly List<UserTask> _tasks;
    public IReadOnlyList<UserTask> Tasks => _tasks;

    public IReadOnlyList<UserTask> CompletedTasks
        => this._tasks.Where(t => !t.IsDeleted && t.IsCompleted).ToList();

    public IReadOnlyList<UserTask> IncompleteTasks
        => this._tasks.Where(t => !t.IsCompleted && !t.IsDeleted).ToList();

    public int IncompleteTaskCount => this.IncompleteTasks.Count;

    private Folder() { }

    private Folder(string title)
    {
        this.Title = title;
        this._tasks = new List<UserTask>();
    }

    public static Folder CreateFolder(FolderChangeData changeData)
    {
        // TODO validate changeData
        return new Folder(changeData.Title);
    }

    public void AddTask(UserTask task)
    {
        if (task is null)
        {
            throw new ArgumentNullException(paramName: nameof(task));
        }

        if (!this._tasks.Contains(task))
        {
            this._tasks.Add(task);
        }
    }
}
