using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с папками
/// </summary>
[ApiController]
[Route("api/folders")]
public class FolderController(TaskService _taskService) : ControllerBase
{
    /// <summary>
    /// Получить папку по идентификатору
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    [HttpGet("{folderId:int}", Name = nameof(GetFolderById))]
    public async Task<IActionResult> GetFolderById(int folderId)
    {
        var folder = await _taskService.GetFolderById(folderId);
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

    /// <summary>
    /// Создать папку
    /// </summary>
    /// <param name="folderDto">Данные для создания папки</param>
    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] FolderForCreationDto folderDto)
    {
        var folder = await _taskService.CreateFolder(folderDto);
        var folderVm = new FolderVm(folder);

        return CreatedAtRoute(
            routeName: nameof(GetFolderById),
            routeValues: new { folderId = folderVm.Id },
            value: folderVm);
    }

    /// <summary>
    /// Обновить папку
    /// </summary>
    /// <param name="folderId">Id папки</param>
    /// <param name="folderDto">Данные для обновления папки</param>
    [HttpPut("{folderId:int}")]
    public async Task<IActionResult> UpdateFolder(
        int folderId, [FromBody] FolderForUpdateDto folderDto)
    {
        var folder = await _taskService.UpdateFolder(folderId, folderDto);
        var folderVm = new FolderVm(folder);

        return Ok(folderVm);
    }

    /// <summary>
    /// Удалить папку
    /// </summary>
    /// <param name="folderId">Id папки</param>
    [HttpDelete("{folderId:int}")]
    public async Task<IActionResult> DeleteFolder(int folderId)
    {
        await _taskService.DeleteFolder(folderId);
        return NoContent();
    }
}
