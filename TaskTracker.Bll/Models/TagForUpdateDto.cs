namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для обновления тега
/// </summary>
public record TagForUpdateDto(
    string Title,
    string Color,
    DateTime? ModifiedDateTime);
