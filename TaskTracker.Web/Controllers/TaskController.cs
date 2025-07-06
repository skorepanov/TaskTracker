using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с задачами
/// </summary>
[ApiController]
[Route("api/tasks")]
public class TaskController(TaskService taskService)
    : ControllerBase
{
    /// <summary>
    /// Получить задачу по идентификатору
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpGet("{taskId:int}", Name = nameof(GetTaskById))]
    public async Task<IActionResult> GetTaskById(int taskId)
    {
        var task = await taskService.GetTaskById(taskId);
        var response = ApiResponse<UserTaskVm>.Success(task);
        return Ok(response);
    }

    /// <summary>
    /// Получить невыполненные задачи
    /// </summary>
    [HttpGet]
    [Route("incomplete")]
    public async Task<IActionResult> GetIncompletedTasks()
    {
        var tasks = await taskService.GetIncompletedTasks();
        var response = ApiResponse<IReadOnlyList<UserTaskVm>>.Success(tasks);
        return Ok(response);
    }

    /// <summary>
    /// Получить выполненные задачи
    /// </summary>
    [HttpGet]
    [Route("complete")]
    public async Task<ActionResult> GetCompletedTasks()
    {
        var tasks = await taskService.GetCompletedTasks();
        var response = ApiResponse<IReadOnlyList<UserTaskVm>>.Success(tasks);
        return Ok(response);
    }

    /// <summary>
    /// Получить задачи из корзины
    /// </summary>
    [HttpGet]
    [Route("trash")]
    public async Task<IActionResult> GetTasksInTrash()
    {
        var tasks = await taskService.GetTasksInTrash();
        var response = ApiResponse<IReadOnlyList<UserTaskVm>>.Success(tasks);
        return Ok(response);
    }

    /// <summary>
    /// Создать задачу
    /// </summary>
    /// <param name="userTaskDto">Данные для создания задачи</param>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] UserTaskForCreationDto userTaskDto)
    {
        var task = await taskService.CreateTask(userTaskDto);
        var response = ApiResponse<UserTaskVm>.Success(task);

        return CreatedAtRoute(
            routeName: nameof(GetTaskById),
            routeValues: new { taskId = task.Id },
            value: response);
    }

    /// <summary>
    /// Обновить задачу
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для обновления задачи</param>
    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> UpdateTask(
        int taskId, [FromBody] UserTaskForUpdateDto userTaskDto)
    {
        var task = await taskService.UpdateTask(taskId, userTaskDto);
        var response = ApiResponse<UserTaskVm>.Success(task);
        return Ok(response);
    }

    /// <summary>
    /// Отметить задачу как выполненную
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для отметки задачи как выполненной</param>
    [HttpPut("{taskId:int}/completed")]
    public async Task<IActionResult> CompleteTask(
        int taskId, [FromBody] UserTaskForCompleteDto userTaskDto)
    {
        var task = await taskService.CompleteTask(taskId, userTaskDto);
        var response = ApiResponse<UserTaskVm>.Success(task);
        return Ok(response);
    }

    /// <summary>
    /// Отметить задачу как невыполненную
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для отметки задачи как невыполненной</param>
    [HttpPut("{taskId:int}/incompleted")]
    public async Task<IActionResult> IncompleteTask(
        int taskId, [FromBody] UserTaskForIncompleteDto userTaskDto)
    {
        var task = await taskService.IncompleteTask(taskId, userTaskDto);
        var response = ApiResponse<UserTaskVm>.Success(task);
        return Ok(response);
    }

    /// <summary>
    /// Переместить задачу в корзину
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для перемещения задачи в корзину</param>
    /// <returns></returns>
    [HttpPut("{taskId:int}/movedToTrash")]
    public async Task<IActionResult> MoveTaskToTrash(
        int taskId, [FromBody] UserTaskForMoveToTrashDto userTaskDto)
    {
        var task = await taskService.MoveTaskToTrash(taskId, userTaskDto);
        var response = ApiResponse<UserTaskVm>.Success(task);
        return Ok(response);
    }

    /// <summary>
    /// Переместить задачу из корзины
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для перемещения задачи из корзины</param>
    [HttpPut("{taskId:int}/movedFromTrash")]
    public async Task<IActionResult> MoveTaskFromTrash(
        int taskId, [FromBody] UserTaskForMoveFromTrashDto userTaskDto)
    {
        var task = await taskService.MoveTaskFromTrash(taskId, userTaskDto);
        var response = ApiResponse<UserTaskVm>.Success(task);
        return Ok(response);
    }

    /// <summary>
    /// Удалить задачу
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        await taskService.DeleteTask(taskId);
        var response = ApiResponse<UserTaskVm>.Success(null);
        return Ok(response);
    }
}
