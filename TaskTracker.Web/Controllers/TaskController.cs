using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

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
    public async Task<IActionResult> CreateTask([FromBody] UserTaskChangeData changeData)
    {
        var task = await _taskService.CreateTask(changeData);
        var taskVm = new UserTaskVm(task, DateTime.Now);
        return Ok(taskVm);
    }

    [HttpGet]
    [Route("today")]
    public async Task<IActionResult> GetTodayTasks()
    {
        var tasks = await _taskService.GetTodayTasks();
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }

    [HttpGet]
    [Route("deleted")]
    public async Task<IActionResult> GetDeletedTasks()
    {
        var tasks = await _taskService.GetDeletedTasks();
        var taskVms = DeletedUserTaskVm.CreateCollectionFrom(tasks);
        return Ok(taskVms);
    }
}
