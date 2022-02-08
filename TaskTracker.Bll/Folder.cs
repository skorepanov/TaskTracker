namespace TaskTracker.Bll;

public class Folder
{
    public int Id { get; private set; }
    public string Title { get; private set; }

    private readonly List<UserTask> _tasks;
    public IReadOnlyCollection<UserTask> Tasks => _tasks;

    public IReadOnlyCollection<UserTask> CompletedTasks
        => this._tasks.Where(t => !t.IsDeleted && t.IsCompleted).ToList();

    public IReadOnlyCollection<UserTask> IncompleteTasks
        => this._tasks.Where(t => !t.IsCompleted && !t.IsDeleted).ToList();

    public int IncompleteTaskCount => this.IncompleteTasks.Count;

    private Folder() { }

    public Folder(int id, string title)
    {
        this.Id = id;
        this.Title = title;
        this._tasks = new List<UserTask>();
    }

    public Folder(string title)
        : this(id: default, title)
    { }

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
