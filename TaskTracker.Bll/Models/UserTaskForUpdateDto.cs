namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для редактирования задачи
/// </summary>
public record UserTaskForUpdateDto(
    int Id,
    string Title,
    string Description,
    DateTime? DueDate,
    int FolderId);
