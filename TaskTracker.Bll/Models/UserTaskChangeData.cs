namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные создаваемой или редактируемой задачи
/// </summary>
public record UserTaskChangeData
{
    /// <summary>
    /// Id задачи
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Наименование задачи
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Описание задачи
    /// </summary>
    public string Description { get; }

    /// <summary>
    /// Планируемая дата выполнения задачи
    /// </summary>
    public DateTime? DueDate { get; }

    /// <summary>
    /// Id папки
    /// </summary>
    public int FolderId { get; }

    public UserTaskChangeData(int id, string title, string description,
        DateTime? dueDate, int folderId)
    {
        this.Id = id;
        this.Title = title;
        this.Description = description;
        this.DueDate = dueDate;
        this.FolderId = folderId;
    }
}
