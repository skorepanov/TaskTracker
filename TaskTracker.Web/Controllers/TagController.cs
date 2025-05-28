using TaskTracker.Web.Models;

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
        var tagVm = new TagVm(tag);
        return Ok(tagVm);
    }

    /// <summary>
    /// Получить все теги
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetTags()
    {
        var tags = await _taskService.GetTags();
        var tagVms = TagVm.CreateCollectionFrom(tags);
        return Ok(tagVms);
    }

    /// <summary>
    /// Создать тег
    /// </summary>
    /// <param name="tagDto">Данные для создания тега</param>
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagForCreationDto tagDto)
    {
        var tag = await _taskService.CreateTag(tagDto);
        var tagVm = new TagVm(tag);

        return CreatedAtRoute(
            routeName: nameof(GetTagById),
            routeValues: new { tagId = tagVm.Id },
            value: tagVm);
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
