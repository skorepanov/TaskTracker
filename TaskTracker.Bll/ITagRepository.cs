using System.Threading.Tasks;

namespace TaskTracker.Bll;

public interface ITagRepository
{
    Task<IReadOnlyList<Tag>> GetTags();
}