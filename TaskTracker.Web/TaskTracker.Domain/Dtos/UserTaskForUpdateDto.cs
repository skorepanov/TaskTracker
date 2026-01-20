namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для редактирования задачи
/// </summary>
public record UserTaskForUpdateDto(
    string? Title,
    string? Description,
    int? FolderId,
    List<int>? TagIds,
    DateTime? DueDateTime,
    DateTime? ModifiedDateTime);
