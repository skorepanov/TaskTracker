using System.Threading.Tasks;

namespace TaskTracker.Bll;

public interface ITagRepository
{
    Task<Tag?> GetTag(int tagId);
    Task<IReadOnlyList<Tag>> GetTags();

    Task DeleteTag(Tag tag);
}
