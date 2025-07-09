namespace TaskTracker.Bll.Services;

public class TagService(
    ITagRepository tagRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<Result<TagVm>> GetTagById(int tagId)
    {
        var tag = await tagRepository.GetTag(tagId);

        if (tag is null)
        {
            return Result.Failure<TagVm>(
                error: $"Тег не обнаружен (id = {tagId})");
        }

        var tagVm = new TagVm(tag);
        var result = Result<TagVm>.Success(tagVm);
        return result;
    }

    public async Task<Result<IReadOnlyList<TagVm>>> GetTags()
    {
        var tags = await tagRepository.GetTags();
        var tagVms = tags.Select(t => new TagVm(t)).ToList();
        var result = Result<IReadOnlyList<TagVm>>.Success(tagVms);
        return result;
    }

    public async Task<Result<TagVm>> CreateTag(TagForCreationDto tagDto)
    {
        var newTag = Tag.CreateTag(tagDto, dateTimeProvider.UtcNow);
        await tagRepository.CreateTag(newTag);

        var newTagVm = new TagVm(newTag);
        var result = Result<TagVm>.Success(newTagVm);
        return result;
    }

    public async Task<Result<TagVm>> UpdateTag(int tagId, TagForUpdateDto tagDto)
    {
        var tag = await tagRepository.GetTag(tagId);

        if (tag is null)
        {
            return Result.Failure<TagVm>(
                error: $"Тег не обнаружен (id = {tagId})");
        }

        tag.UpdateTag(tagDto, dateTimeProvider.UtcNow);
        await tagRepository.UpdateTag(tag);

        var tagVm = new TagVm(tag);
        var result = Result<TagVm>.Success(tagVm);
        return result;
    }

    public async Task<Result> DeleteTag(int tagId)
    {
        var tag = await tagRepository.GetTag(tagId);

        if (tag is null)
        {
            return Result.Failure<TagVm>(
                error: $"Тег не обнаружен (id = {tagId})");
        }

        await tagRepository.DeleteTag(tag);

        var result = Result.Success();
        return result;
    }
}
