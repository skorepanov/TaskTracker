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
        var response = ApiResponse<Folder>.Success(folder);
        return Ok(response);
    }

    /// <summary>
    /// Получить все папки
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFolders()
    {
        var folders = await _taskService.GetFolders();
        var folderVms = folders.Select(f => new FolderVm(f)).ToList();
        var response = ApiResponse<IReadOnlyList<FolderVm>>.Success(folderVms);
        return Ok(response);
    }

    /// <summary>
    /// Создать папку
    /// </summary>
    /// <param name="folderDto">Данные для создания папки</param>
    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] FolderForCreationDto folderDto)
    {
        var folder = await _taskService.CreateFolder(folderDto);
        var response = ApiResponse<Folder>.Success(folder);

        return CreatedAtRoute(
            routeName: nameof(GetFolderById),
            routeValues: new { folderId = folder.Id },
            value: response);
    }

    /// <summary>
    /// Обновить папку
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    /// <param name="folderDto">Данные для обновления папки</param>
    [HttpPut("{folderId:int}")]
    public async Task<IActionResult> UpdateFolder(
        int folderId, [FromBody] FolderForUpdateDto folderDto)
    {
        var folder = await _taskService.UpdateFolder(folderId, folderDto);
        var response = ApiResponse<Folder>.Success(folder);
        return Ok(response);
    }

    /// <summary>
    /// Удалить папку
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    [HttpDelete("{folderId:int}")]
    public async Task<IActionResult> DeleteFolder(int folderId)
    {
        await _taskService.DeleteFolder(folderId);
        var response = ApiResponse<Tag>.Success(null);
        return Ok(response);
    }
}
