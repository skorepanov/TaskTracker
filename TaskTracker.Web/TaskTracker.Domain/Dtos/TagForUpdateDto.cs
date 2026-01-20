namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для обновления тега
/// </summary>
public record TagForUpdateDto(
    string? Title,
    string? Color,
    DateTime? ModifiedDateTime);
