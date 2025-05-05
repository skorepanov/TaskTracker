namespace TaskTracker.Bll.Models;

/// <summary>
/// Данные для перемещения задачи в корзину
/// </summary>
public record UserTaskForMoveToTrashDto(
    DateTime? MovedToTrashDateTime);