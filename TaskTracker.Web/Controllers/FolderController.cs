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
        return Ok(folder);
    }

    /// <summary>
    /// Получить все папки
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFolders()
    {
        var folders = await _taskService.GetFolders();
        return Ok(folders);
    }

    /// <summary>
    /// Создать папку
    /// </summary>
    /// <param name="folderDto">Данные для создания папки</param>
    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] FolderForCreationDto folderDto)
    {
        var folder = await _taskService.CreateFolder(folderDto);

        return CreatedAtRoute(
            routeName: nameof(GetFolderById),
            routeValues: new { folderId = folder.Id },
            value: folder);
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
        return Ok(folder);
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
