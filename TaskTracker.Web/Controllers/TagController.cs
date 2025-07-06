using TaskTracker.Web.Models;

namespace TaskTracker.Web.Controllers;

/// <summary>
/// Работа с тегами задач
/// </summary>
[ApiController]
[Route("api/tags")]
public class TagController(TagService _tagService) : ControllerBase
{
    /// <summary>
    /// Получить тег по идентификатору
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpGet("{tagId:int}", Name = nameof(GetTagById))]
    public async Task<IActionResult> GetTagById(int tagId)
    {
        var tag = await _tagService.GetTagById(tagId);
        var tagVm = new TagVm(tag);
        var response = ApiResponse<TagVm>.Success(tagVm);
        return Ok(response);
    }

    /// <summary>
    /// Получить все теги
    /// </summary>
    [HttpGet]
    public async Task<ActionResult> GetTags()
    {
        var tags = await _tagService.GetTags();
        var tagVms = tags.Select(t => new TagVm(t)).ToList();
        var response = ApiResponse<IReadOnlyList<TagVm>>.Success(tagVms);
        return Ok(response);
    }

    /// <summary>
    /// Создать тег
    /// </summary>
    /// <param name="tagDto">Данные для создания тега</param>
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] TagForCreationDto tagDto)
    {
        var tag = await _tagService.CreateTag(tagDto);
        var tagVm = new TagVm(tag);
        var response = ApiResponse<TagVm>.Success(tagVm);

        return CreatedAtRoute(
            routeName: nameof(GetTagById),
            routeValues: new { tagId = tagVm.Id },
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
        var tag = await _tagService.UpdateTag(tagId, tagDto);
        var tagVm = new TagVm(tag);
        var response = ApiResponse<TagVm>.Success(tagVm);
        return Ok(response);
    }

    /// <summary>
    /// Удалить тег
    /// </summary>
    /// <param name="tagId">Идентификатор тега</param>
    [HttpDelete("{tagId:int}")]
    public async Task<IActionResult> DeleteTag(int tagId)
    {
        await _tagService.DeleteTag(tagId);
        var response = ApiResponse<TagVm>.Success(null);
        return Ok(response);
    }
}
