namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с папками
/// </summary>
[ApiController]
[Route("api/folders")]
public class FolderController(ExecutionService executionService, FolderService folderService)
    : ControllerBase
{
    /// <summary>
    /// Получить папку по идентификатору
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    [HttpGet("{folderId:int}", Name = nameof(GetFolderById))]
    public async Task<IActionResult> GetFolderById(int folderId)
    {
        var result = await executionService.TryExecute(
            () => folderService.GetFolderById(folderId));
        return Ok(result);
    }

    /// <summary>
    /// Получить все папки
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFolders()
    {
        var result = await executionService.TryExecute(
            () => folderService.GetFolders());
        return Ok(result);
    }

    /// <summary>
    /// Создать папку
    /// </summary>
    /// <param name="folderDto">Данные для создания папки</param>
    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] FolderForCreationDto folderDto)
    {
        var result = await executionService.TryExecute(
            () => folderService.CreateFolder(folderDto));
        return Ok(result);
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
        var result = await executionService.TryExecute(
            () => folderService.UpdateFolder(folderId, folderDto));
        return Ok(result);
    }

    /// <summary>
    /// Удалить папку
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    [HttpDelete("{folderId:int}")]
    public async Task<IActionResult> DeleteFolder(int folderId)
    {
        var result = await executionService.TryExecute(
            () => folderService.DeleteFolder(folderId));
        return Ok(result);
    }
}
