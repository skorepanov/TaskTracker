namespace TaskTracker.Application.Interfaces;

public interface ITagRepository
{
    Task<Tag> GetTag(int tagId);

    Task<IReadOnlyList<Tag>> GetTags();

    Task<IReadOnlyList<Tag>> GetTags(ICollection<int> tagIds);

    Task CreateTag(Tag tag);

    Task UpdateTag(Tag tag);

    Task DeleteTag(Tag tag);
}
