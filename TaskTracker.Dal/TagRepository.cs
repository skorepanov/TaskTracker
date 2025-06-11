using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;
using TaskTracker.Bll.Models;

namespace TaskTracker.Dal;

public class TagRepository(ApplicationContext _db) : ITagRepository
{
    public async Task<Tag?> GetTag(int tagId)
    {
        return await _db.Tags.SingleOrDefaultAsync(t => t.Id == tagId);
    }

    public async Task<IReadOnlyList<Tag>> GetTags()
    {
        return await _db.Tags.ToListAsync();
    }

    public async Task<IReadOnlyList<Tag>> GetTags(IEnumerable<int> tagIds)
    {
        return await _db.Tags.Where(t => tagIds.Contains(t.Id)).ToListAsync();
    }

    public async Task CreateTag(Tag tag)
    {
        _db.Tags.Add(tag);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateTag(Tag tag)
    {
        _db.Tags.Update(tag);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteTag(Tag tag)
    {
        _db.Tags.Remove(tag);
        await _db.SaveChangesAsync();
    }
}
