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
    /// Количество невыполненных задач в папке
    /// </summary>
    public int IncompleteTaskCount { get; }

    /// <summary>
    /// Сконструировать представление для папки
    /// </summary>
    public FolderVm(Folder folder)
    {
        this.Id = folder.Id;
        this.Title = folder.Title;
        this.IncompleteTaskCount = folder.IncompleteTaskCount;
    }

    /// <summary>
    /// Сформировать коллекцию представлений для папок
    /// </summary>
    public static IReadOnlyList<FolderVm> CreateCollectionFrom(IEnumerable<Folder> folders)
    {
        return folders.Select(f => new FolderVm(f)).ToList();
    }
}
