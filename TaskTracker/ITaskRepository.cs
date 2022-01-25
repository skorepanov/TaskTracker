namespace TaskTracker;

public interface ITaskRepository
{
    Folder GetFolder(int folderId);

    UserTask SaveNewTask(UserTask task, int folderId);
    Folder SaveNewFolder(Folder folder);
}
