namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для обновления папки
/// </summary>
public record FolderForUpdateDto(
    string? Title,
    DateTime? ModifiedDateTime);
