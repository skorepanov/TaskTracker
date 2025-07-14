namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для отметки задачи как невыполненной
/// </summary>
public record UserTaskForIncompleteDto(
    DateTime? ModifiedDateTime);