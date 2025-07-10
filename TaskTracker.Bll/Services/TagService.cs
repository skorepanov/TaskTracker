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
        return Result<TagVm>.Success(tagVm);
    }

    public async Task<Result<IReadOnlyList<TagVm>>> GetTags()
    {
        var tags = await tagRepository.GetTags();
        var tagVms = tags.Select(t => new TagVm(t)).ToList();
        return Result<IReadOnlyList<TagVm>>.Success(tagVms);
    }

    public async Task<Result<TagVm>> CreateTag(TagForCreationDto tagDto)
    {
        var newTagResult = Tag.CreateTag(tagDto, dateTimeProvider.UtcNow);

        if (!newTagResult.IsOk)
        {
            return Result.Failure<TagVm>(newTagResult.Error ?? string.Empty);
        }

        await tagRepository.CreateTag(newTagResult.Value);

        var newTagVm = new TagVm(newTagResult.Value);
        return Result<TagVm>.Success(newTagVm);
    }

    public async Task<Result<TagVm>> UpdateTag(int tagId, TagForUpdateDto tagDto)
    {
        var tag = await tagRepository.GetTag(tagId);

        if (tag is null)
        {
            return Result.Failure<TagVm>(
                error: $"Тег не обнаружен (id = {tagId})");
        }

        var updateResult = tag.UpdateTag(tagDto, dateTimeProvider.UtcNow);

        if (!updateResult.IsOk)
        {
            return Result.Failure<TagVm>(updateResult.Error ?? string.Empty);
        }

        await tagRepository.UpdateTag(tag);

        var tagVm = new TagVm(tag);
        return Result<TagVm>.Success(tagVm);
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

        return Result.Success();
    }
}
