namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для отметки задачи как выполненной
/// </summary>
public record UserTaskForCompleteDto(
    DateTime? CompletedDateTime);
