using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с папками
/// </summary>
[ApiController]
[Route("api/folders")]
public class FolderController : ControllerBase
{
    private readonly TaskService _taskService;

    /// <summary>
    /// Сконструировать контроллер для работы с папками
    /// </summary>
    public FolderController(TaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Создать папку
    /// </summary>
    /// <param name="changeData">Данные создаваемой папки</param>
    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] FolderChangeData changeData)
    {
        var folder = await _taskService.CreateFolder(changeData);
        var folderVm = new FolderVm(folder);
        return Ok(folderVm);
    }

    /// <summary>
    /// Получить все папки
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFolders()
    {
        var folders = await _taskService.GetFolders();
        var folderVms = FolderVm.CreateCollectionFrom(folders);
        return Ok(folderVms);
    }

    /// <summary>
    /// Получить невыполненные задачи для папки
    /// </summary>
    /// <param name="folderId">Id папки</param>
    [HttpGet]
    [Route("{folderId:int}/incompleteTasks")]
    public async Task<IActionResult> GetIncompleteTasks(int folderId)
    {
        var tasks = await _taskService.GetIncompleteTasks(folderId);
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }

    /// <summary>
    /// Получить выполненные задачи для папки
    /// </summary>
    /// <param name="folderId">Id папки</param>
    [HttpGet]
    [Route("{folderId:int}/completedTasks")]
    public async Task<IActionResult> GetCompletedTasks(int folderId)
    {
        var tasks = await _taskService.GetCompletedTasks(folderId);
        var taskVms = UserTaskVm.CreateCollectionFrom(tasks, DateTime.Now);
        return Ok(taskVms);
    }
}
