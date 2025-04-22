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
    /// Фактическая дата и время выполнения задачи
    /// </summary>
    public DateTime? CompletedDateTime { get; }

    /// <summary>
    /// Выполнена ли задача
    /// </summary>
    public bool IsCompleted { get; }

    /// <summary>
    /// Планируемая дата выполнения задачи
    /// </summary>
    public DateTime? DueDate { get; }

    /// <summary>
    /// Количество дней, на которое просрочена задача
    /// </summary>
    public int OverdueDaysCount { get; }

    /// <summary>
    /// Дата удаления задачи
    /// </summary>
    public DateTime? DeletionDate { get; }

    /// <summary>
    /// Удалена ли задача
    /// </summary>
    public bool IsDeleted { get; }

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
        DueDate = task.DueDate;
        OverdueDaysCount = task.CalculateOverdueDays(today);
        DeletionDate = task.DeletionDate;
        IsDeleted = task.IsDeleted;
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
