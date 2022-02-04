using Microsoft.AspNetCore.Mvc;
using TaskTracker.Bll;

namespace TaskTracker.Web;

[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService taskService)
    {
        this._taskService = taskService;
    }

    [Route("today")]
    public IReadOnlyCollection<UserTaskVm> GetTodayTasks()
    {
        var tasks = _taskService.GetTodayTasks();
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return taskVms;
    }

    [Route("deleted")]
    public IReadOnlyCollection<DeletedUserTaskVm> GetDeletedTasks()
    {
        var tasks = _taskService.GetDeletedTasks();
        var taskVms = DeletedUserTaskVm.CreateCollectionFrom(tasks);
        return taskVms;
    }
}
