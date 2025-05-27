using Microsoft.EntityFrameworkCore;
using TaskTracker.Bll;
using TaskTracker.Bll.Models;

namespace TaskTracker.Dal;

public class TagRepository(ApplicationContext _db) : ITagRepository
{
    public async Task<IReadOnlyList<Tag>> GetTags()
    {
        return await _db.Tags.ToListAsync();
    }
}
