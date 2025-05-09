namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для редактирования задачи
/// </summary>
public record UserTaskForUpdateDto(
    string Title,
    string? Description,
    int? FolderId,
    DateTime? DueDateTime,
    DateTime? ModifiedDateTime);
