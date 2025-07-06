namespace TaskTracker.Bll;

public class TagService(
    ITagRepository tagRepository,
    IDateTimeProvider dateTimeProvider)
{
    public async Task<Tag> GetTagById(int tagId)
    {
        var tag = await tagRepository.GetTag(tagId);

        if (tag is null)
        {
            throw new DomainEntityNotFoundException(
                domainEntityType: typeof(Tag),
                message: $"Тег не обнаружен (id = {tagId})");
        }

        return tag;
    }

    public async Task<IReadOnlyList<Tag>> GetTags()
    {
        var tags = await tagRepository.GetTags();
        return tags;
    }

    public async Task<Tag> CreateTag(TagForCreationDto tagDto)
    {
        var newTag = Tag.CreateTag(tagDto, dateTimeProvider.UtcNow);
        await tagRepository.CreateTag(newTag);

        return newTag;
    }

    public async Task<Tag> UpdateTag(int tagId, TagForUpdateDto tagDto)
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

        return tag;
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
