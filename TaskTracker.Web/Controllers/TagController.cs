using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с тегами задач
/// </summary>
[ApiController]
[Route("api/tags")]
public class TagController(TagService tagService) : ControllerBase
{
    /// <summary>
    /// Получить тег по идентификатору
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpGet("{tagId:int}", Name = nameof(GetTagById))]
    public async Task<IActionResult> GetTagById(int tagId)
    {
        var tag = await tagService.GetTagById(tagId);
        var response = ApiResponse<TagVm>.Success(tag);
        return Ok(response);
    }

    /// <summary>
    /// Получить все теги
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetTags()
    {
        var tags = await tagService.GetTags();
        var response = ApiResponse<IReadOnlyList<TagVm>>.Success(tags);
        return Ok(response);
    }

    /// <summary>
    /// Создать тег
    /// </summary>
    /// <param name="tagDto">Данные для создания тега</param>
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagForCreationDto tagDto)
    {
        var tag = await tagService.CreateTag(tagDto);
        var response = ApiResponse<TagVm>.Success(tag);

        return CreatedAtRoute(
            routeName: nameof(GetTagById),
            routeValues: new { tagId = tag.Id },
            value: response);
    }

    /// <summary>
    /// Обновить тег
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    /// <param name="tagDto">Данные для обновления тега</param>
    [HttpPut("{tagId:int}")]
    public async Task<IActionResult> UpdateTag(
        int tagId, [FromBody] TagForUpdateDto tagDto)
    {
        var tag = await tagService.UpdateTag(tagId, tagDto);
        var response = ApiResponse<TagVm>.Success(tag);
        return Ok(response);
    }

    /// <summary>
    /// Удалить тег
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpDelete("{tagId:int}")]
    public async Task<IActionResult> DeleteTag(int tagId)
    {
        await tagService.DeleteTag(tagId);
        var response = ApiResponse<TagVm>.Success(null);
        return Ok(response);
    }
}
