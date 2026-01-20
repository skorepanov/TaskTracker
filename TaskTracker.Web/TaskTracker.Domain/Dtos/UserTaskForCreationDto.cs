namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для создания задачи
/// </summary>
public record UserTaskForCreationDto(
    string? Title,
    int? FolderId,
    DateTime? DueDateTime,
    DateTime? CreatedDateTime);
