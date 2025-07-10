namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для обновления папки
/// </summary>
public record FolderForUpdateDto(
    string? Title,
    DateTime? ModifiedDateTime);