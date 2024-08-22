namespace TaskTracker.Web.Models;

/// <summary>
/// Задача
/// </summary>
public class UserTaskVm
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
    /// Фактическая дата выполнения задачи
    /// </summary>
    public DateTime? CompletionDate { get; }

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

    public UserTaskVm(UserTask task, DateTime today)
    {
        this.Id = task.Id;
        this.Title = task.Title;
        this.Description = task.Description;
        this.CompletionDate = task.CompletionDate;
        this.IsCompleted = task.IsCompleted;
        this.DueDate = task.DueDate;
        this.OverdueDaysCount = task.CalculateOverdueDays(today);
        this.DeletionDate = task.DeletionDate;
        this.IsDeleted = task.IsDeleted;
    }

    public static IReadOnlyCollection<UserTaskVm> CreateCollectionFrom(
        IEnumerable<UserTask> tasks, DateTime today)
    {
        return tasks.Select(t => new UserTaskVm(t, today)).ToList();
    }
}
