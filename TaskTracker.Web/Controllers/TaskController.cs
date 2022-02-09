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
    public IActionResult GetTodayTasks()
    {
        var tasks = _taskService.GetTodayTasks();
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }

    [Route("deleted")]
    public IActionResult GetDeletedTasks()
    {
        var tasks = _taskService.GetDeletedTasks();
        var taskVms = DeletedUserTaskVm.CreateCollectionFrom(tasks);
        return Ok(taskVms);
    }
}
