namespace TaskTracker.Web.Models;

/// <summary>
/// Представление для тега
/// </summary>
public class TagVm
{
    /// <summary>
    /// Id тега
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Наименование тега
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Цвет тега (Hex)
    /// </summary>
    public string Color { get; private set; }

    /// <summary>
    /// Дата создания тега
    /// </summary>
    public DateTime CreatedDateTime { get; private set; }

    /// <summary>
    /// Дата изменения тега
    /// </summary>
    public DateTime? ModifiedDateTime { get; private set; }

    /// <summary>
    /// Сконструировать представление для тега
    /// </summary>
    public TagVm(Tag tag)
    {
        Id = tag.Id;
        Title = tag.Title;
        Color = tag.Color;
        CreatedDateTime = tag.CreatedDateTime;
        ModifiedDateTime = tag.ModifiedDateTime;
    }

    /// <summary>
    /// Сформировать коллекцию представлений для тегов
    /// </summary>
    public static IReadOnlyList<TagVm> CreateCollectionFrom(IEnumerable<Tag> tags)
    {
        return tags.Select(t => new TagVm(t)).ToList();
    }
}
