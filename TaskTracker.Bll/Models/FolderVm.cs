using System.Text.Json.Serialization;

namespace TaskTracker.Bll.Models;

/// <summary>
/// Представление для папки
/// </summary>
public class FolderVm
{
    /// <summary>
    /// Идентификатор папки
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Наименование папки
    /// </summary>
    public string Title { get; init; }

    /// <summary>
    /// Дата создания папки
    /// </summary>
    public DateTime CreatedDateTime { get; init; }

    /// <summary>
    /// Дата изменения папки
    /// </summary>
    public DateTime? ModifiedDateTime { get; init; }

    /// <summary>
    /// Сконструировать представление для папки
    /// </summary>
    public FolderVm(Folder folder)
    {
        Id = folder.Id;
        Title = folder.Title;
        CreatedDateTime = folder.CreatedDateTime;
        ModifiedDateTime = folder.ModifiedDateTime;
    }

    /// <summary>
    /// Сконструировать представление для папки (для парсинга JSON)
    /// </summary>
    [JsonConstructor]
    public FolderVm(
        int id,
        string title,
        DateTime createdDateTime,
        DateTime? modifiedDateTime)
    {
        Id = id;
        Title = title;
        CreatedDateTime = createdDateTime;
        ModifiedDateTime = modifiedDateTime;
    }
}