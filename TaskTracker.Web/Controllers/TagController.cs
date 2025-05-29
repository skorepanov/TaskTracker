namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с тегами задач
/// </summary>
[ApiController]
[Route("api/tags")]
public class TagController(TaskService _taskService) : ControllerBase
{
    /// <summary>
    /// Получить тег по идентификатору
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpGet("{tagId:int}", Name = nameof(GetTagById))]
    public async Task<IActionResult> GetTagById(int tagId)
    {
        var tag = await _taskService.GetTagById(tagId);
        return Ok(tag);
    }

    /// <summary>
    /// Получить все теги
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetTags()
    {
        var tags = await _taskService.GetTags();
        return Ok(tags);
    }

    /// <summary>
    /// Создать тег
    /// </summary>
    /// <param name="tagDto">Данные для создания тега</param>
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagForCreationDto tagDto)
    {
        var tag = await _taskService.CreateTag(tagDto);

        return CreatedAtRoute(
            routeName: nameof(GetTagById),
            routeValues: new { tagId = tag.Id },
            value: tag);
    }

    /// <summary>
    /// Обновить тег
    /// </summary>
    /// <param name="tagId">Id тега</param>
    /// <param name="tagDto">Данные для обновления тега</param>
    [HttpPut("{tagId:int}")]
    public async Task<IActionResult> UpdateTag(
        int tagId, [FromBody] TagForUpdateDto tagDto)
    {
        var tag = await _taskService.UpdateTag(tagId, tagDto);
        return Ok(tag);
    }

    /// <summary>
    /// Удалить тег
    /// </summary>
    /// <param name="tagId">Id тега</param>
    [HttpDelete("{tagId:int}")]
    public async Task<IActionResult> DeleteTag(int tagId)
    {
        await _taskService.DeleteTag(tagId);
        return NoContent();
    }
}
