using System.Threading.Tasks;

namespace TaskTracker.Bll;

public interface ITagRepository
{
    Task<Tag?> GetTag(int tagId);
    Task<IReadOnlyList<Tag>> GetTags();

    Task CreateTag(Tag tag);

    Task UpdateTag(Tag tag);

    Task DeleteTag(Tag tag);
}
