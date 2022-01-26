﻿namespace TaskTracker;

public interface ITaskRepository
{
    UserTask GetTask(int taskId);
    Folder GetFolder(int folderId);

    void SaveNewTask(UserTask task, int folderId);
    void SaveNewFolder(Folder folder);

    void UpdateTaskFolder(int taskId, int folderId);
}
