using System.Text.Json.Serialization;

namespace TaskTracker.Bll.Models;

/// <summary>
/// Представление для задачи
/// </summary>
public record UserTaskVm
{
    /// <summary>
    /// Идентификатор задачи
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Наименование задачи
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// Описание задачи
    /// </summary>
    public string? Description { get; }

    /// <summary>
    /// Идентификатор папки
    /// </summary>
    public int? FolderId { get; }

    /// <summary>
    /// Дата и время выполнения задачи
    /// </summary>
    public DateTime? CompletedDateTime { get; }

    /// <summary>
    /// Планируемая дата и время выполнения задачи
    /// </summary>
    public DateTime? DueDateTime { get; }

    /// <summary>
    /// Количество дней, на которое просрочена задача
    /// </summary>
    public int OverdueDaysCount { get; }

    /// <summary>
    /// Дата перемещения задачи в корзину
    /// </summary>
    public DateTime? MovedToTrashDateTime { get; }

    /// <summary>
    /// Идентификаторы тегов
    /// </summary>
    public IReadOnlyList<int>? TagIds { get; }

    /// <summary>
    /// Дата создания задачи
    /// </summary>
    public DateTime CreatedDateTime { get; }

    /// <summary>
    /// Дата изменения задачи
    /// </summary>
    public DateTime? ModifiedDateTime { get; }

    /// <summary>
    /// Сконструировать представление для задачи
    /// </summary>
    public UserTaskVm(UserTask task, DateTime today)
    {
        Id = task.Id;
        Title = task.Title;
        Description = task.Description;
        FolderId = task.FolderId;
        CompletedDateTime = task.CompletedDateTime;
        DueDateTime = task.DueDateTime;
        OverdueDaysCount = task.CalculateOverdueDays(today);
        MovedToTrashDateTime = task.MovedToTrashDateTime;
        TagIds = task.Tags?.Count > 0 ? task.Tags.Select(t => t.Id).ToList() : null;
        CreatedDateTime = task.CreatedDateTime;
        ModifiedDateTime = task.ModifiedDateTime;
    }

    /// <summary>
    /// Сконструировать представление для задачи (для парсинга JSON)
    /// </summary>
    [JsonConstructor]
    public UserTaskVm(
        int id,
        string title,
        string? description,
        int? folderId,
        DateTime? completedDateTime,
        DateTime? dueDateTime,
        int overdueDaysCount,
        DateTime? movedToTrashDateTime,
        IReadOnlyList<int>? tagIds,
        DateTime createdDateTime,
        DateTime? modifiedDateTime)
    {
        Id = id;
        Title = title;
        Description = description;
        FolderId = folderId;
        CompletedDateTime = completedDateTime;
        DueDateTime = dueDateTime;
        OverdueDaysCount = overdueDaysCount;
        MovedToTrashDateTime = movedToTrashDateTime;
        TagIds = tagIds;
        CreatedDateTime = createdDateTime;
        ModifiedDateTime = modifiedDateTime;
    }
}
