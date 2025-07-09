namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с папками
/// </summary>
[ApiController]
[Route("api/folders")]
public class FolderController(FolderService folderService) : ControllerBase
{
    /// <summary>
    /// Получить папку по идентификатору
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    [HttpGet("{folderId:int}", Name = nameof(GetFolderById))]
    public async Task<IActionResult> GetFolderById(int folderId)
    {
        var folder = await folderService.GetFolderById(folderId);
        var result = Result<FolderVm>.Success(folder);
        return Ok(result);
    }

    /// <summary>
    /// Получить все папки
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetFolders()
    {
        var folders = await folderService.GetFolders();
        var result = Result<IReadOnlyList<FolderVm>>.Success(folders);
        return Ok(result);
    }

    /// <summary>
    /// Создать папку
    /// </summary>
    /// <param name="folderDto">Данные для создания папки</param>
    [HttpPost]
    public async Task<IActionResult> CreateFolder([FromBody] FolderForCreationDto folderDto)
    {
        var folder = await folderService.CreateFolder(folderDto);
        var result = Result<FolderVm>.Success(folder);

        return CreatedAtRoute(
            routeName: nameof(GetFolderById),
            routeValues: new { folderId = folder.Id },
            value: result);
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
        var folder = await folderService.UpdateFolder(folderId, folderDto);
        var result = Result<FolderVm>.Success(folder);
        return Ok(result);
    }

    /// <summary>
    /// Удалить папку
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    [HttpDelete("{folderId:int}")]
    public async Task<IActionResult> DeleteFolder(int folderId)
    {
        await folderService.DeleteFolder(folderId);
        var result = Result<FolderVm>.Success(null);
        return Ok(result);
    }
}
