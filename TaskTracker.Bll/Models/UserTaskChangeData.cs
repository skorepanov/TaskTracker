namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные создаваемой или редактируемой задачи
/// </summary>
public record UserTaskChangeData(
    int Id,
    string Title,
    string Description,
    DateTime? DueDate,
    int FolderId);
