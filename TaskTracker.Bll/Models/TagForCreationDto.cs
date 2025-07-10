namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для создания тега
/// </summary>
public record TagForCreationDto(
    string? Title,
    string? Color,
    DateTime? CreatedDateTime);