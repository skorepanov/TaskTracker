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
    public async Task<ActionResult<FolderVm>> GetFolderById(int folderId)
    {
        var folder = await folderService.GetFolderById(folderId);
        return Ok(folder);
    }

    /// <summary>
    /// Получить все папки
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<FolderVm>>> GetFolders()
    {
        var folders = await folderService.GetFolders();
        return Ok(folders);
    }

    /// <summary>
    /// Создать папку
    /// </summary>
    /// <param name="folderDto">Данные для создания папки</param>
    [HttpPost]
    public async Task<ActionResult<FolderVm>> CreateFolder(
        [FromBody] FolderForCreationDto folderDto)
    {
        var folder = await folderService.CreateFolder(folderDto);
        return Ok(folder);
    }

    /// <summary>
    /// Обновить папку
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    /// <param name="folderDto">Данные для обновления папки</param>
    [HttpPut("{folderId:int}")]
    public async Task<ActionResult<FolderVm>> UpdateFolder(
        int folderId, [FromBody] FolderForUpdateDto folderDto)
    {
        var folder = await folderService.UpdateFolder(folderId, folderDto);
        return Ok(folder);
    }

    /// <summary>
    /// Удалить папку
    /// </summary>
    /// <param name="folderId">Идентификатор папки</param>
    [HttpDelete("{folderId:int}")]
    public async Task<ActionResult<bool>> DeleteFolder(int folderId)
    {
        var isDeleted = await folderService.DeleteFolder(folderId);
        return Ok(isDeleted);
    }
}
