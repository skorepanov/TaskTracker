namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные создаваемой или редактируемой папки
/// </summary>
public record FolderChangeData
{
    /// <summary>
    /// Id папки
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Наименование папки
    /// </summary>
    public string Title { get; }

    public FolderChangeData(int id, string title)
    {
        this.Id = id;
        this.Title = title;
    }
}

