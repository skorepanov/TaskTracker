namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для создания папки
/// </summary>
public record FolderForCreationDto(
    string? Title,
    DateTime? CreatedDateTime);
