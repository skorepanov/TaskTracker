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
