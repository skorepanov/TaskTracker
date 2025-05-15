using System.Threading.Tasks;

namespace TaskTracker.Bll;

public interface ITaskRepository
{
    Task<UserTask?> GetTask(int taskId);
    Task<IReadOnlyList<UserTask>> GetIncompletedTasks();
    Task<IReadOnlyList<UserTask>> GetCompletedTasks();

    Task<IReadOnlyList<UserTask>> GetNonMovedToTrashTasks();
    Task<IReadOnlyList<UserTask>> GetTasksInInbox();
    Task<IReadOnlyList<UserTask>> GetTasksInTrash();

    Task CreateTask(UserTask task);

    Task UpdateTask(UserTask task);

    Task DeleteTask(UserTask task);
}
