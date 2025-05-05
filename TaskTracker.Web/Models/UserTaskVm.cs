namespace TaskTracker.Web.Models;

/// <summary>
/// Представление для задачи
/// </summary>
public record UserTaskVm
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
    /// Дата и время выполнения задачи
    /// </summary>
    public DateTime? CompletedDateTime { get; }

    /// <summary>
    /// Выполнена ли задача
    /// </summary>
    public bool IsCompleted { get; }

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
    /// Находится ли задача в корзине
    /// </summary>
    public bool IsInTrash { get; }

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
        CompletedDateTime = task.CompletedDateTime;
        IsCompleted = task.IsCompleted;
        DueDateTime = task.DueDateTime;
        OverdueDaysCount = task.CalculateOverdueDays(today);
        MovedToTrashDateTime = task.MovedToTrashDateTime;
        IsInTrash = task.IsInTrash;
        CreatedDateTime = task.CreatedDateTime;
        ModifiedDateTime = task.ModifiedDateTime;
    }

    /// <summary>
    /// Сформировать коллекцию представлений для задач
    /// </summary>
    public static IReadOnlyList<UserTaskVm> CreateCollectionFrom(
        IEnumerable<UserTask> tasks, DateTime today)
    {
        return tasks.Select(t => new UserTaskVm(t, today)).ToList();
    }
}
