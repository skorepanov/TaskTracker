using System.Threading.Tasks;

namespace TaskTracker.Bll;

public interface ITaskRepository
{
    Task<UserTask> GetTask(int taskId);
    Task<IReadOnlyList<UserTask>> GetNonDeletedTasks();
    Task<IReadOnlyList<UserTask>> GetDeletedTasks();

    Task<Folder> GetFolder(int folderId);
    Task<IReadOnlyList<Folder>> GetFolders();

    Task SaveNewTask(UserTask task, int folderId);
    Task SaveNewFolder(Folder folder);

    Task UpdateTask(UserTask task);
    Task UpdateTaskFolder(int taskId, int folderId);
}
