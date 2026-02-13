namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с тегами задач
/// </summary>
[ApiController]
[Route("api/tags")]
public class TagController(TagService tagService)
    : ControllerBase
{
    /// <summary>
    /// Получить тег по идентификатору
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpGet("{tagId:int}", Name = nameof(GetTagById))]
    public async Task<ActionResult<TagVm>> GetTagById(int tagId)
    {
        var tag = await tagService.GetTagById(tagId);
        return Ok(tag);
    }

    /// <summary>
    /// Получить все теги
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TagVm>>> GetTags()
    {
        var tags = await tagService.GetTags();
        return Ok(tags);
    }

    /// <summary>
    /// Создать тег
    /// </summary>
    /// <param name="tagDto">Данные для создания тега</param>
    [HttpPost]
    public async Task<ActionResult<TagVm>> CreateTag(
        [FromBody] TagForCreationDto tagDto)
    {
        var tag = await tagService.CreateTag(tagDto);
        return Ok(tag);
    }

    /// <summary>
    /// Обновить тег
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    /// <param name="tagDto">Данные для обновления тега</param>
    [HttpPut("{tagId:int}")]
    public async Task<ActionResult<TagVm>> UpdateTag(
        int tagId, [FromBody] TagForUpdateDto tagDto)
    {
        var tag = await tagService.UpdateTag(tagId, tagDto);
        return Ok(tag);
    }

    /// <summary>
    /// Удалить тег
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpDelete("{tagId:int}")]
    public async Task<ActionResult<bool>> DeleteTag(int tagId)
    {
        var isDeleted = await tagService.DeleteTag(tagId);
        return Ok(isDeleted);
    }
}
