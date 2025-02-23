namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для создания задачи
/// </summary>
public record UserTaskForCreationDto(
    string Title,
    string Description,
    DateTime? DueDate,
    int? FolderId);