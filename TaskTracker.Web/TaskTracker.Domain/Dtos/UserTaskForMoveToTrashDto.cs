namespace TaskTracker.Domain.Dtos;

/// <summary>
/// Данные для перемещения задачи в корзину
/// </summary>
public record UserTaskForMoveToTrashDto(
    DateTime? MovedToTrashDateTime);