namespace TaskTracker.Dal.Repositories;

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

    public async Task<IReadOnlyList<Tag>> GetTags(IEnumerable<int> tagIds)
    {
        return await db.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync();
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
