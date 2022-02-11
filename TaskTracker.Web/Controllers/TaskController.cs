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

    // TODO method should returns task id only
    [HttpPost]
    public IActionResult CreateTask([FromBody] UserTaskChangeData changeData)
    {
        var task = _taskService.CreateTask(changeData);
        var taskVm = new UserTaskVm(task, DateTime.Now);
        return Ok(taskVm);
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
