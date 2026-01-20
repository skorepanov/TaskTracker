using System.Text.Json.Serialization;

namespace TaskTracker.Application.Vms;

/// <summary>
/// Представление для тега
/// </summary>
public class TagVm
{
    /// <summary>
    /// Идентификатор тега
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Название тега
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Цвет тега
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
    /// Сконструировать представление для тега (для парсинга JSON)
    /// </summary>
    [JsonConstructor]
    public TagVm(
        int id,
        string title,
        string color,
        DateTime createdDateTime,
        DateTime? modifiedDateTime)
    {
        Id = id;
        Title = title;
        Color = color;
        CreatedDateTime = createdDateTime;
        ModifiedDateTime = modifiedDateTime;
    }
}