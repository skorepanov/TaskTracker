namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для создания папки
/// </summary>
public record FolderForCreationDto(
    string Title,
    DateTime? CreatedDateTime);
