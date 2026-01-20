namespace TaskTracker.Infrastructure.Repositories;

public class TagRepository(ApplicationContext db) : ITagRepository
{
    public async Task<Tag> GetTag(int tagId)
    {
        var tag = await db.Tags.SingleOrDefaultAsync(t => t.Id == tagId);

        if (tag is null)
        {
            var error = ErrorMessages.TagNotFound(tagId);
            throw new DomainException(error);
        }

        return tag;
    }

    public async Task<IReadOnlyList<Tag>> GetTags()
    {
        return await db.Tags.ToListAsync();
    }

    public async Task<IReadOnlyList<Tag>> GetTags(ICollection<int> tagIds)
    {
        var tags = await db.Tags
            .Where(t => tagIds.Contains(t.Id))
            .ToListAsync();

        if (tags.Count != tagIds.Count)
        {
            var existentTagIds = tags.Select(t => t.Id).ToList();
            var nonExistentTagIds = tagIds.Except(existentTagIds).ToList();

            var error = ErrorMessages.TagsNotFound(nonExistentTagIds);
            throw new DomainException(error);
        }

        return tags;
    }

    public async Task CreateTag(Tag tag)
    {
        db.Tags.Add(tag);
        await db.SaveChangesAsync();
    }

    public async Task UpdateTag(Tag tag)
    {
        db.Tags.Update(tag);
        await db.SaveChangesAsync();
    }

    public async Task DeleteTag(Tag tag)
    {
        db.Tags.Remove(tag);
        await db.SaveChangesAsync();
    }
}
