namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для отметки задачи как невыполненной
/// </summary>
public record UserTaskForIncompleteDto(
    DateTime? ModifiedDateTime);