namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для отметки задачи как выполненной
/// </summary>
public record UserTaskForCompleteDto(
    DateTime? CompletedDateTime);
