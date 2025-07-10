namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с задачами
/// </summary>
[ApiController]
[Route("api/tasks")]
public class TaskController(TaskService taskService) : ControllerBase
{
    /// <summary>
    /// Получить задачу по идентификатору
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpGet("{taskId:int}", Name = nameof(GetTaskById))]
    public async Task<IActionResult> GetTaskById(int taskId)
    {
        var result = await taskService.GetTaskById(taskId);
        return Ok(result);
    }

    /// <summary>
    /// Получить невыполненные задачи
    /// </summary>
    [HttpGet]
    [Route("incomplete")]
    public async Task<IActionResult> GetIncompletedTasks()
    {
        var result = await taskService.GetIncompletedTasks();
        return Ok(result);
    }

    /// <summary>
    /// Получить выполненные задачи
    /// </summary>
    [HttpGet]
    [Route("complete")]
    public async Task<ActionResult> GetCompletedTasks()
    {
        var result = await taskService.GetCompletedTasks();
        return Ok(result);
    }

    /// <summary>
    /// Получить задачи из корзины
    /// </summary>
    [HttpGet]
    [Route("trash")]
    public async Task<IActionResult> GetTasksInTrash()
    {
        var result = await taskService.GetTasksInTrash();
        return Ok(result);
    }

    /// <summary>
    /// Создать задачу
    /// </summary>
    /// <param name="userTaskDto">Данные для создания задачи</param>
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] UserTaskForCreationDto userTaskDto)
    {
        var result = await taskService.CreateTask(userTaskDto);
        return Ok(result);
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
        var result = await taskService.UpdateTask(taskId, userTaskDto);
        return Ok(result);
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
        var result = await taskService.CompleteTask(taskId, userTaskDto);
        return Ok(result);
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
        var result = await taskService.IncompleteTask(taskId, userTaskDto);
        return Ok(result);
    }

    /// <summary>
    /// Переместить задачу в корзину
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для перемещения задачи в корзину</param>
    [HttpPut("{taskId:int}/movedToTrash")]
    public async Task<IActionResult> MoveTaskToTrash(
        int taskId, [FromBody] UserTaskForMoveToTrashDto userTaskDto)
    {
        var result = await taskService.MoveTaskToTrash(taskId, userTaskDto);
        return Ok(result);
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
        var result = await taskService.MoveTaskFromTrash(taskId, userTaskDto);
        return Ok(result);
    }

    /// <summary>
    /// Удалить задачу
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {
        var result = await taskService.DeleteTask(taskId);
        return Ok(result);
    }
}
