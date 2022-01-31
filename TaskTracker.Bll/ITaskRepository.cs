using System.Collections.Generic;

namespace TaskTracker.Bll;

public interface ITaskRepository
{
    UserTask GetTask(int taskId);
    IReadOnlyCollection<UserTask> GetNonDeletedTasks();

    Folder GetFolder(int folderId);
    IReadOnlyCollection<Folder> GetFolders();

    void SaveNewTask(UserTask task, int folderId);
    void SaveNewFolder(Folder folder);

    void UpdateTask(UserTask task);
    void UpdateTaskFolder(int taskId, int folderId);
}
