namespace TaskTracker.Web.Models;

/// <summary>
/// Представление для папки
/// </summary>
public record FolderVm
{
    /// <summary>
    /// Id папки
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Наименование папки
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Дата создания папки
    /// </summary>
    public DateTime CreatedDateTime { get; }

    /// <summary>
    /// Дата изменения папки
    /// </summary>
    public DateTime? ModifiedDateTime { get; }

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
    /// Сформировать коллекцию представлений для папок
    /// </summary>
    public static IReadOnlyList<FolderVm> CreateCollectionFrom(IEnumerable<Folder> folders)
    {
        return folders.Select(f => new FolderVm(f)).ToList();
    }
}
