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
    public async Task<ActionResult<UserTaskVm>> GetTaskById(int taskId)
    {
        var task = await taskService.GetTaskById(taskId);
        return Ok(task);
    }

    /// <summary>
    /// Получить невыполненные задачи
    /// </summary>
    [HttpGet]
    [Route("incomplete")]
    public async Task<ActionResult<IReadOnlyList<UserTaskVm>>>
        GetIncompletedTasks()
    {
        var tasks = await taskService.GetIncompletedTasks();
        return Ok(tasks);
    }

    /// <summary>
    /// Получить выполненные задачи
    /// </summary>
    [HttpGet]
    [Route("complete")]
    public async Task<ActionResult<IReadOnlyList<UserTaskVm>>> GetCompletedTasks()
    {
        var tasks = await taskService.GetCompletedTasks();
        return Ok(tasks);
    }

    /// <summary>
    /// Получить задачи из корзины
    /// </summary>
    [HttpGet]
    [Route("trash")]
    public async Task<ActionResult<IReadOnlyList<UserTaskVm>>> GetTasksInTrash()
    {
        var tasks = await taskService.GetTasksInTrash();
        return Ok(tasks);
    }

    /// <summary>
    /// Создать задачу
    /// </summary>
    /// <param name="userTaskDto">Данные для создания задачи</param>
    [HttpPost]
    public async Task<ActionResult<UserTaskVm>> CreateTask(
        [FromBody] UserTaskForCreationDto userTaskDto)
    {
        var task = await taskService.CreateTask(userTaskDto);
        return Ok(task);
    }

    /// <summary>
    /// Обновить задачу
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для обновления задачи</param>
    [HttpPut("{taskId:int}")]
    public async Task<ActionResult<UserTaskVm>> UpdateTask(
        int taskId, [FromBody] UserTaskForUpdateDto userTaskDto)
    {
        var task = await taskService.UpdateTask(taskId, userTaskDto);
        return Ok(task);
    }

    /// <summary>
    /// Отметить задачу как выполненную
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для отметки задачи как выполненной</param>
    [HttpPut("{taskId:int}/completed")]
    public async Task<ActionResult<UserTaskVm>> CompleteTask(
        int taskId, [FromBody] UserTaskForCompleteDto userTaskDto)
    {
        var task = await taskService.CompleteTask(taskId, userTaskDto);
        return Ok(task);
    }

    /// <summary>
    /// Отметить задачу как невыполненную
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для отметки задачи как невыполненной</param>
    [HttpPut("{taskId:int}/incompleted")]
    public async Task<ActionResult<UserTaskVm>> IncompleteTask(
        int taskId, [FromBody] UserTaskForIncompleteDto userTaskDto)
    {
        var task = await taskService.IncompleteTask(taskId, userTaskDto);
        return Ok(task);
    }

    /// <summary>
    /// Переместить задачу в корзину
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для перемещения задачи в корзину</param>
    [HttpPut("{taskId:int}/movedToTrash")]
    public async Task<ActionResult<UserTaskVm>> MoveTaskToTrash(
        int taskId, [FromBody] UserTaskForMoveToTrashDto userTaskDto)
    {
        var task = await taskService.MoveTaskToTrash(taskId, userTaskDto);
        return Ok(task);
    }

    /// <summary>
    /// Переместить задачу из корзины
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    /// <param name="userTaskDto">Данные для перемещения задачи из корзины</param>
    [HttpPut("{taskId:int}/movedFromTrash")]
    public async Task<ActionResult<UserTaskVm>> MoveTaskFromTrash(
        int taskId, [FromBody] UserTaskForMoveFromTrashDto userTaskDto)
    {
        var task = await taskService.MoveTaskFromTrash(taskId, userTaskDto);
        return Ok(task);
    }

    /// <summary>
    /// Удалить задачу
    /// </summary>
    /// <param name="taskId">Идентификатор задачи</param>
    [HttpDelete("{taskId:int}")]
    public async Task<ActionResult<bool>> DeleteTask(int taskId)
    {
        var isDeleted = await taskService.DeleteTask(taskId);
        return Ok(isDeleted);
    }
}
