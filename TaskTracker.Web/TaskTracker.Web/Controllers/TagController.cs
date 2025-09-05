namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с тегами задач
/// </summary>
[ApiController]
[Route("api/tags")]
public class TagController(ExecutionService executionService, TagService tagService)
    : ControllerBase
{
    /// <summary>
    /// Получить тег по идентификатору
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpGet("{tagId:int}", Name = nameof(GetTagById))]
    public async Task<ActionResult<Result<TagVm>>> GetTagById(int tagId)
    {
        var result = await executionService.TryExecute(
            () => tagService.GetTagById(tagId));
        return Ok(result);
    }

    /// <summary>
    /// Получить все теги
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<Result<IReadOnlyList<TagVm>>>> GetTags()
    {
        var result = await executionService.TryExecute(
            () => tagService.GetTags());
        return Ok(result);
    }

    /// <summary>
    /// Создать тег
    /// </summary>
    /// <param name="tagDto">Данные для создания тега</param>
    [HttpPost]
    public async Task<ActionResult<Result<TagVm>>> CreateTag(
        [FromBody] TagForCreationDto tagDto)
    {
        var result = await executionService.TryExecute(
            () => tagService.CreateTag(tagDto));
        return Ok(result);
    }

    /// <summary>
    /// Обновить тег
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    /// <param name="tagDto">Данные для обновления тега</param>
    [HttpPut("{tagId:int}")]
    public async Task<ActionResult<Result<TagVm>>> UpdateTag(
        int tagId, [FromBody] TagForUpdateDto tagDto)
    {
        var result = await executionService.TryExecute(
            () => tagService.UpdateTag(tagId, tagDto));
        return Ok(result);
    }

    /// <summary>
    /// Удалить тег
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpDelete("{tagId:int}")]
    public async Task<ActionResult<Result<bool>>> DeleteTag(int tagId)
    {
        var result = await executionService.TryExecute(
            () => tagService.DeleteTag(tagId));
        return Ok(result);
    }
}
