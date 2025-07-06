namespace TaskTracker.Bll;

public class TagService(
    ITagRepository tagRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<TagVm> GetTagById(int tagId)
    {
        var tag = await tagRepository.GetTag(tagId);

        if (tag is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Tag),
                message: $"Тег не обнаружен (id = {tagId})");
        }

        var tagVm = new TagVm(tag);
        return tagVm;
    }

    public async Task<IReadOnlyList<TagVm>> GetTags()
    {
        var tags = await tagRepository.GetTags();
        var tagVms = tags.Select(t => new TagVm(t)).ToList();
        return tagVms;
    }

    public async Task<TagVm> CreateTag(TagForCreationDto tagDto)
    {
        var newTag = Tag.CreateTag(tagDto, dateTimeProvider.UtcNow);
        await tagRepository.CreateTag(newTag);

        var newTagVm = new TagVm(newTag);
        return newTagVm;
    }

    public async Task<TagVm> UpdateTag(int tagId, TagForUpdateDto tagDto)
    {
        var tag = await tagRepository.GetTag(tagId);

        if (tag is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Tag),
                message: $"Тег не обнаружен (id = {tagId})");
        }

        tag.UpdateTag(tagDto, dateTimeProvider.UtcNow);
        await tagRepository.UpdateTag(tag);

        var tagVm = new TagVm(tag);
        return tagVm;
    }

    public async Task DeleteTag(int tagId)
    {
        var tag = await tagRepository.GetTag(tagId);

        if (tag is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Tag),
                message: $"Тег не обнаружен (id = {tagId})");
        }

        await tagRepository.DeleteTag(tag);
    }
}
