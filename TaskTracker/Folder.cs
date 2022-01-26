using System;
using System.Collections.Generic;

namespace TaskTracker;

public class Folder
{
    public int Id { get; }
    public string Title { get; }
    public List<UserTask> Tasks { get; }

    public Folder(int id, string title)
    {
        this.Id = id;
        this.Title = title;
        this.Tasks = new List<UserTask>();
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

        if (!this.Tasks.Contains(task))
        {
            this.Tasks.Add(task);
        }
    }
}
