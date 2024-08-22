using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с задачами
/// </summary>
[ApiController]
[Route("api/tasks")]
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService taskService)
    {
        this._taskService = taskService;
    }

    /// <summary>
    /// Создать задачу
    /// </summary>
    /// <param name="changeData">Данные создаваемой задачи</param>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] UserTaskChangeData changeData)
    {
        var task = await _taskService.CreateTask(changeData);
        var taskVm = new UserTaskVm(task, DateTime.Now);
        return Ok(taskVm);
    }

    /// <summary>
    /// Получить задачи на сегодня
    /// </summary>
    [HttpGet]
    [Route("today")]
    public async Task<IActionResult> GetTodayTasks()
    {
        var tasks = await _taskService.GetTodayTasks();
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }

    /// <summary>
    /// Получить удалённые задачи
    /// </summary>
    [HttpGet]
    [Route("deleted")]
    public async Task<IActionResult> GetDeletedTasks()
    {
        var tasks = await _taskService.GetDeletedTasks();
        var taskVms = DeletedUserTaskVm.CreateCollectionFrom(tasks);
        return Ok(taskVms);
    }
}
