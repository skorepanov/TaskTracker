﻿using System;

namespace TaskTracker;

public class UserTask : IComparable
{
    public int Id { get; }
    public string Title { get; }
    public string Description { get; }

    public DateTime? CompletionDate { get; private set; }

    public bool IsCompleted => CompletionDate is not null;

    public UserTask(int id, string title, string description)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
    }

    public UserTask(string title, string description)
        : this(id: default, title, description)
    { }

    public void Complete(DateTime completionDate)
    {
        this.CompletionDate = completionDate;
    }

    public void Incomplete()
    {
        this.CompletionDate = null;
    }

    public override bool Equals(object obj)
    {
        UserTask task = (UserTask)obj;
        return this.Id == task?.Id;
    }

    public int CompareTo(object obj)
    {
        throw new NotImplementedException();
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
