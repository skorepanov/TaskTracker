using System.Threading.Tasks;

namespace TaskTracker.Bll;

public interface ITaskRepository
{
    Task<UserTask?> GetTask(int taskId);
    Task<IReadOnlyList<UserTask>> GetNonDeletedTasks();
    Task<IReadOnlyList<UserTask>> GetTasksInTrash();

    Task CreateTask(UserTask task, int folderId);

    Task UpdateTask(UserTask task);
    Task UpdateTaskFolder(int taskId, int folderId);

    Task DeleteTaskPermanently(UserTask task);
}
