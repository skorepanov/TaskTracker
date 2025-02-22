using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с задачами
/// </summary>
[ApiController]
[Route("api/tasks")]
public class TaskController(TaskService _taskService) : ControllerBase
{
    /// <summary>
    /// Получить задачу по идентификатору
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpGet("{taskId:int}", Name = nameof(GetTaskById))]
    public async Task<IActionResult> GetTaskById(int taskId)
    {
        var task = await _taskService.GetTaskById(taskId);
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
    /// Получить задачи из корзины
    /// </summary>
    [HttpGet]
    [Route("deleted")]
    public async Task<IActionResult> GetTasksInTrash()
    {
        var tasks = await _taskService.GetTasksInTrash();
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }

    /// <summary>
    /// Создать задачу
    /// </summary>
    /// <param name="userTaskDto">Данные для создания задачи</param>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] UserTaskForCreationDto userTaskDto)
    {
        var task = await _taskService.CreateTask(userTaskDto);
        var taskVm = new UserTaskVm(task, DateTime.Now);

        return CreatedAtRoute(routeName: nameof(GetTaskById),
            routeValues: new { id = taskVm.Id },
            value: taskVm);
    }

    /// <summary>
    /// Обновить задачу
    /// </summary>
    /// <param name="taskId">Id задачи</param>
    /// <param name="userTaskDto">Данные для обновления задачи</param>
    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> UpdateTask(
        int taskId, [FromBody] UserTaskForUpdateDto userTaskDto)
    {
        var task = await _taskService.UpdateTask(taskId, userTaskDto);
        var taskVm = new UserTaskVm(task, DateTime.Now);

        return Ok(taskVm);
    }

    /// <summary>
    /// Удалить задачу перманентно
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteTaskPermanently(int taskId)
    {
        await _taskService.DeleteTaskPermanently(taskId);
        return NoContent();
    }
}
